using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TemplateWeb.Component;
using TemplateWeb.Models.DB;

namespace TemplateWeb.Controllers
{
    [MemberAuthorize]
    public class MemberController : Controller
    {
        EntityDB entity = new EntityDB();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Welcome()
        {
            return View();
        }
        public ActionResult Layout_Get()
        {
            var member = MemberManager.GetMember();
            var member_messageCount = 0;
            if (member != null)
            {
                member_messageCount = entity.member_message.Count(p => p.member_id == member.id && p.state_read == false);
            }
            return Json(new
            {
                member,
                member_messageCount,
            }, JsonRequestBehavior.AllowGet);
        }

        #region 登陆
        [AllowAnonymous]
        public ActionResult Login(string redirectUrl)
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string phone, string password)
        {
            account_member member = entity.account_member.FirstOrDefault(p => p.phone == phone);
            if (member != null && DESTool.Encrypt(password) == member.password)
            {
                if (member.enable == true)
                {
                    HttpContext.Session["tpmember"] = member;
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("账号已被停用", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("账号或密码错误", JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("tpmember");
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region 注册
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult SendSMSCode(string phone)
        {
            SMSTool tool = new SMSTool();
            string code = tool.CreateCode(phone);
            string expireTime = tool.expireTime.ToString();
            bool codeResult = tool.SendCode(phone, 100278, new string[] { code, expireTime });
            if (codeResult == true)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("验证码发送失败", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(string phone, string password, string code)
        {
            account_member member = entity.account_member.FirstOrDefault(p => p.phone == phone);
            if (member == null)
            {
                SMSTool tool = new SMSTool();
                bool codeResult = tool.CheckCode(phone, code);
                if (codeResult == true)
                {
                    account_member new_member = new account_member()
                    {
                        enable = true,
                        password = DESTool.Encrypt(password),
                        phone = phone,
                        sys_datetime = DateTime.Now,
                    };
                    entity.account_member.Add(new_member);
                    if (entity.SaveChanges() > 0)
                    {
                        HttpContext.Session["tpmember"] = new_member;
                        MessageTool.SendMessage(new_member.id, "系统通知", "恭喜您注册成功！");
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    return Json("用户创建失败", JsonRequestBehavior.AllowGet);
                }
                return Json("验证码无效");
            }
            return Json("账号已被注册", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 设置
        #region 个人信息
        public ActionResult Info()
        {
            return View();
        }
        public ActionResult Info_Get()
        {
            int id = MemberManager.GetMember().id;
            account_member member = entity.account_member.FirstOrDefault(p => p.id == id);
            return Json(member, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Info_Add_Edit(account_member memberModel)
        {
            var query = entity.account_member.FirstOrDefault(p => p.id == memberModel.id);
            query.real_name = memberModel.real_name;
            query.sex = memberModel.sex;
            query.idcard_number = memberModel.idcard_number;
            query.email = memberModel.email;
            query.remark = memberModel.remark;
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 密码管理
        public ActionResult Password()
        {
            return View();
        }
        public ActionResult Password_Add_Edit(string password)
        {
            int id = MemberManager.GetMember().id;
            account_member member = entity.account_member.FirstOrDefault(p => p.id == id);
            member.password = DESTool.Encrypt(password);
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region 消息
        public ActionResult MessageList()
        {
            return View();
        }
        public ActionResult MessageList_Get()
        {
            int id = MemberManager.GetMember().id;
            var query = entity.member_message.Where(p => p.member_id == id).OrderByDescending(p => p.id).ToArray().Select(p => new
            {
                p.id,
                p.content,
                p.state_read,
                p.title,
                sys_datetime = p.sys_datetime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            });
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Message_Add_Edit(member_message messageaModel)
        {
            var query = entity.member_message.FirstOrDefault(p => p.id == messageaModel.id);
            query.state_read = messageaModel.state_read;
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Message_Delete(int id)
        {
            var query = entity.member_message.FirstOrDefault(p => p.id == id);
            entity.member_message.Remove(query);
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 订单
        public ActionResult OrderList()
        {
            return View();
        }
        public ActionResult OrderList_Get()
        {
            int id = MemberManager.GetMember().id;
            var query = entity.pay_order.Where(p => p.member_id == id && p.state_pay == true && p.delete == false).OrderByDescending(p => p.id).ToArray().Join(entity.module_product, a => a.product_id, b => b.id, (a, b) => new
            {
                a.id,
                a.delete,
                a.member_id,
                a.number,
                a.pay_method,
                pay_time = a.pay_time.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                a.price,
                a.product_id,
                a.remark,
                a.state_pay,
                sys_datetime = a.sys_datetime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                b.name,
                b.attachment,
            });
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Order_Delete(int id)
        {
            var query = entity.pay_order.FirstOrDefault(p => p.id == id);
            query.delete = true;
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 产品
        public ActionResult ProductList()
        {
            return View();
        }
        public ActionResult ProductList_Get()
        {
            int id = MemberManager.GetMember().id;
            var query = entity.pay_order.Where(p => p.member_id == id && p.state_pay == true).OrderByDescending(p => p.id).ToArray().Join(entity.module_product, a => a.product_id, b => b.id, (a, b) => new
            {
                a.id,
                a.delete,
                a.member_id,
                a.number,
                a.pay_method,
                pay_time = a.pay_time.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                a.price,
                a.product_id,
                a.remark,
                a.state_pay,
                sys_datetime = a.sys_datetime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                b.name,
                b.attachment,
            });
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}