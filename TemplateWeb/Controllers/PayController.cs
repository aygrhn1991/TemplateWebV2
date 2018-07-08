using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TemplateWeb.Component;
using TemplateWeb.Component.WxExtension;
using TemplateWeb.Models.DB;
using WxPayAPI;

namespace TemplateWeb.Controllers
{
    [MemberAuthorize]
    public class PayController : Controller
    {
        EntityDB entity = new EntityDB();
        #region 订单辅助方法
        private pay_order CreateOrder(int productId, string payMethod)
        {
            int id = MemberManager.GetMember().id;
            module_product product = entity.module_product.FirstOrDefault(p => p.id == productId);
            pay_order order = new pay_order()
            {
                member_id = id,
                product_id = productId,
                pay_time = null,
                price = product.price,
                delete = false,
                number = id.ToString().PadRight(15, '0') + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                pay_method = payMethod,
                remark = null,
                state_pay = false,
                sys_datetime = DateTime.Now,
            };
            entity.pay_order.Add(order);
            entity.SaveChanges();
            return order;
        }
        #endregion
        #region 预支付
        public ActionResult PrePay(int productId)
        {
            module_product product = entity.module_product.FirstOrDefault(p => p.id == productId);
            var setting = entity.lay_setting;
            ViewBag.productId = productId;
            ViewBag.name = product.name;
            ViewBag.price = product.price;
            ViewBag.logo = setting.FirstOrDefault(p => p.key == "logo").value;
            ViewBag.sitename = setting.FirstOrDefault(p => p.key == "sitename").value;
            return View();
        }
        #endregion
        #region 微信支付
        public ActionResult WxPay(int productId, string payMethod)
        {
            pay_order order = this.CreateOrder(productId, payMethod);
            module_product product = entity.module_product.FirstOrDefault(p => p.id == order.product_id);
            var setting = entity.lay_setting;

            WxPayData data = new WxPayData();
            data.SetValue("body", setting.FirstOrDefault(p => p.key == "sitename").value + product.name);//商品描述
            data.SetValue("attach", "attach");//附加数据
            data.SetValue("out_trade_no", order.number);//随机字符串
            data.SetValue("total_fee", (int)Math.Ceiling(order.price.Value * 100));//总金额
            //data.SetValue("total_fee", 1);//总金额
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
            data.SetValue("goods_tag", "goods_tag");//商品标记
            data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("product_id", order.product_id);//商品ID
            WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
            string url = QRTool.CreateQR(result.GetValue("code_url").ToString());//获得统一下单接口返回的二维码链接
            ViewBag.url = url;

            ViewBag.name = product.name;
            ViewBag.price = product.price;
            ViewBag.logo = setting.FirstOrDefault(p => p.key == "logo").value;
            ViewBag.sitename = setting.FirstOrDefault(p => p.key == "sitename").value;
            return View();
        }
        #endregion
        #region 微信支付结果通知
        [AllowAnonymous]
        public ActionResult WxPayNotify()
        {
            WxPayData notifyData = GetNotifyData();
            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                HttpContext.Response.Write(res.ToXml());
                HttpContext.Response.End();
            }
            string transaction_id = notifyData.GetValue("transaction_id").ToString();
            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                HttpContext.Response.Write(res.ToXml());
                HttpContext.Response.End();
            }
            //查询订单成功
            else
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                HttpContext.Response.Write(res.ToXml());
                HttpContext.Response.End();
                //本地业务处理
                string number = notifyData.GetValue("out_trade_no").ToString();
                pay_order order = entity.pay_order.FirstOrDefault(p => p.number == number);
                module_product product = entity.module_product.FirstOrDefault(p => p.id == order.product_id);
                if (order.state_pay != true)
                {
                    order.pay_time = DateTime.Now;
                    order.state_pay = true;
                    int result = entity.SaveChanges();
                    MessageTool.SendMessage(order.member_id.Value, "购买通知", "您已成功购买【" + product.name + "】！");
                    //页面同步跳转
                    //var hub = GlobalHost.ConnectionManager.GetHubContext<WxPayHub>().Clients.Client("12");//此处需要找到一种方法进行一对一通讯，暂时没想到办法
                    var hub = GlobalHost.ConnectionManager.GetHubContext<WxPayHub>().Clients;
                    hub.All.payResult("success");
                }
            }
            return null;
        }
        #endregion
        #region 微信支付辅助方法
        private WxPayData GetNotifyData()
        {
            //接收从微信后台POST过来的数据
            System.IO.Stream s = HttpContext.Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();
            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                HttpContext.Response.Write(res.ToXml());
                HttpContext.Response.End();
            }
            return data;
        }
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region 支付宝支付
        public ActionResult AliPay(int productId, string payMethod)
        {
            pay_order order = this.CreateOrder(productId, payMethod);
            module_product product = entity.module_product.FirstOrDefault(p => p.id == order.product_id);
            var setting = entity.lay_setting;

            DefaultAopClient client = new DefaultAopClient(Config.gatewayUrl, Config.app_id, Config.private_key, "json", "1.0", Config.sign_type, Config.alipay_public_key, Config.charset, false);
            // 外部订单号，商户网站订单系统中唯一的订单号
            string out_trade_no = order.number;
            // 订单名称
            string subject = setting.FirstOrDefault(p => p.key == "sitename").value + product.name;
            // 付款金额
            //string total_amout = Math.Ceiling(order.price.Value * 100).ToString();
            string total_amout = 0.01.ToString();
            // 商品描述
            string body = setting.FirstOrDefault(p => p.key == "sitename").value + product.name;
            // 组装业务参数model
            AlipayTradePagePayModel model = new AlipayTradePagePayModel();
            model.Body = body;
            model.Subject = subject;
            model.TotalAmount = total_amout;
            model.OutTradeNo = out_trade_no;
            model.ProductCode = "FAST_INSTANT_TRADE_PAY";
            AlipayTradePagePayRequest request = new AlipayTradePagePayRequest();
            // 设置同步回调地址
            request.SetReturnUrl(HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + "/Member/Index");
            // 设置异步通知接收地址
            request.SetNotifyUrl(HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + "/Pay/AliPayNotify");
            // 将业务model载入到request
            request.SetBizModel(model);
            AlipayTradePagePayResponse response = null;
            try
            {
                response = client.pageExecute(request, null, "post");
                Response.Write(response.Body);
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return null;
        }
        #endregion
        [AllowAnonymous]
        public ActionResult AliPayNotify()
        {
            /* 实际验证过程建议商户添加以下校验。
        1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
        2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
        3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
        4、验证app_id是否为该商户本身。
        */
            Dictionary<string, string> sArray = GetRequestPost();
            //本地业务处理
            string number = sArray["out_trade_no"];
            pay_order order = entity.pay_order.FirstOrDefault(p => p.number == number);
            module_product product = entity.module_product.FirstOrDefault(p => p.id == order.product_id);
            if (order.state_pay != true)
            {
                order.pay_time = DateTime.Now;
                order.state_pay = true;
                int result = entity.SaveChanges();
                MessageTool.SendMessage(order.member_id.Value, "购买通知", "您已成功购买【" + product.name + "】！");
            }
            //本地业务处理 结束
            if (sArray.Count != 0)
            {
                bool flag = AlipaySignature.RSACheckV1(sArray, Config.alipay_public_key, Config.charset, Config.sign_type, false);
                if (flag)
                {
                    //交易状态
                    //判断该笔订单是否在商户网站中已经做过处理
                    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                    //请务必判断请求时的total_amount与通知时获取的total_fee为一致的
                    //如果有做过处理，不执行商户的业务程序

                    //注意：
                    //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                    string trade_status = Request.Form["trade_status"];
                    
                    Response.Write("success");
                }
                else
                {
                    Response.Write("fail");
                }
            }
            return null;
        }
        private Dictionary<string, string> GetRequestPost()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //coll = Request.Form;
            coll = Request.Form;
            String[] requestItem = coll.AllKeys;
            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }
            return sArray;

        }
    }
}