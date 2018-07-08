using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TemplateWeb.Component;
using TemplateWeb.Models.DB;

namespace TemplateWeb.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
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

        #region 登陆
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string phone, string password)
        {
            account_admin admin = entity.account_admin.FirstOrDefault(p => p.phone == phone);
            if (admin != null && DESTool.Encrypt(password) == admin.password)
            {
                if (admin.enable == true)
                {
                    HttpContext.Session["tpadmin"] = admin;
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
            HttpContext.Session.Remove("tpadmin");
            return RedirectToAction("Login", "Admin");
        }
        #endregion

        #region 单页管理
        public ActionResult PageList()
        {
            return View();
        }
        public ActionResult PageAdd()
        {
            return View();
        }
        public ActionResult PageList_Get()
        {
            var query = entity.lay_page.OrderBy(p => p.id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Page_Get(int id)
        {
            var query = entity.lay_page.FirstOrDefault(p => p.id == id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        public ActionResult Page_Add_Edit(lay_page pageModel)
        {
            if (pageModel.id == 0)
            {
                lay_page page = new lay_page()
                {
                    title = pageModel.title,
                    content = pageModel.content,
                    sys_datetime = DateTime.Now
                };
                entity.lay_page.Add(page);
            }
            else
            {
                var query = entity.lay_page.FirstOrDefault(p => p.id == pageModel.id);
                query.title = pageModel.title;
                query.content = pageModel.content;
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Page_Delete(int id)
        {
            var query = entity.lay_page.FirstOrDefault(p => p.id == id);
            entity.lay_page.Remove(query);
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 导航管理
        #region 导航管理
        public ActionResult NavList()
        {
            return View();
        }
        public ActionResult NavList_Get()
        {
            var query = entity.lay_nav_nav.OrderBy(p => p.sort);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Nav_Add_Edit(lay_nav_nav navModel)
        {
            if (navModel.id == 0)
            {
                var query = entity.lay_nav_nav;
                int maxSort = query.Count() <= 0 ? 0 : query.Max(p => p.sort.Value);
                lay_nav_nav nav = new lay_nav_nav()
                {
                    title = navModel.title,
                    enable = navModel.enable,
                    mode = navModel.mode,
                    sort = ++maxSort,
                    page_id = navModel.page_id,
                    url = navModel.url,
                    sys_datetime = DateTime.Now
                };
                entity.lay_nav_nav.Add(nav);
            }
            else
            {
                var query = entity.lay_nav_nav.FirstOrDefault(p => p.id == navModel.id);
                query.title = navModel.title;
                query.enable = navModel.enable;
                query.mode = navModel.mode;
                query.sort = navModel.sort;
                query.page_id = navModel.page_id;
                query.url = navModel.url;
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Nav_Delete(int id)
        {
            var query = entity.lay_nav_nav.FirstOrDefault(p => p.id == id);
            entity.lay_nav_nav.Remove(query);
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Nav_Sort(int id, string sortType)
        {
            var query = entity.lay_nav_nav.OrderBy(p => p.sort).ToArray();
            for (int i = 0; i < query.Count(); i++)
            {
                if (query[i].id == id)
                {
                    if (sortType == "up")
                    {
                        if (i == 0)
                        {
                            break;
                        }
                        else
                        {
                            int tempSort = query[i].sort.Value;
                            query[i].sort = query[i - 1].sort;
                            query[i - 1].sort = tempSort;
                        }
                    }
                    else
                    {
                        if (i == query.Count() - 1)
                        {
                            break;
                        }
                        else
                        {
                            int tempSort = query[i].sort.Value;
                            query[i].sort = query[i + 1].sort;
                            query[i + 1].sort = tempSort;
                        }
                    }
                }
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 导航内容管理
        public ActionResult NavContent(int id)
        {
            return View();
        }
        public ActionResult Nav_Get(int id)
        {
            var query = entity.lay_nav_nav.FirstOrDefault(p => p.id == id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 子导航管理
        public ActionResult SubnavList_Get(int id)
        {
            var query = entity.lay_nav_subnav.Where(p => p.nav_id == id).OrderBy(p => p.sort);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Subnav_Add_Edit(lay_nav_subnav subnavModel)
        {
            if (subnavModel.id == 0)
            {
                var query = entity.lay_nav_subnav.Where(p => p.nav_id == subnavModel.nav_id);
                int maxSort = query.Count() <= 0 ? 0 : query.Max(p => p.sort.Value);
                lay_nav_subnav subnav = new lay_nav_subnav()
                {
                    nav_id = subnavModel.nav_id,
                    title = subnavModel.title,
                    enable = subnavModel.enable,
                    mode = subnavModel.mode,
                    sort = ++maxSort,
                    page_id = subnavModel.page_id,
                    url = subnavModel.url,
                    sys_datetime = DateTime.Now
                };
                entity.lay_nav_subnav.Add(subnav);
            }
            else
            {
                var query = entity.lay_nav_subnav.FirstOrDefault(p => p.id == subnavModel.id);
                query.nav_id = subnavModel.nav_id;
                query.title = subnavModel.title;
                query.enable = subnavModel.enable;
                query.mode = subnavModel.mode;
                query.sort = subnavModel.sort;
                query.page_id = subnavModel.page_id;
                query.url = subnavModel.url;
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Subnav_Delete(int id)
        {
            var query = entity.lay_nav_subnav.FirstOrDefault(p => p.id == id);
            entity.lay_nav_subnav.Remove(query);
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Subnav_Sort(int id, string sortType)
        {
            var query = entity.lay_nav_subnav.OrderBy(p => p.sort).ToArray();
            for (int i = 0; i < query.Count(); i++)
            {
                if (query[i].id == id)
                {
                    if (sortType == "up")
                    {
                        if (i == 0)
                        {
                            break;
                        }
                        else
                        {
                            int tempSort = query[i].sort.Value;
                            query[i].sort = query[i - 1].sort;
                            query[i - 1].sort = tempSort;
                        }
                    }
                    else
                    {
                        if (i == query.Count() - 1)
                        {
                            break;
                        }
                        else
                        {
                            int tempSort = query[i].sort.Value;
                            query[i].sort = query[i + 1].sort;
                            query[i + 1].sort = tempSort;
                        }
                    }
                }
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 子导航内容管理
        public ActionResult NavsubContent(int id)
        {
            return View();
        }
        public ActionResult Subnav_Get(int id)
        {
            var query = entity.lay_nav_subnav.FirstOrDefault(p => p.id == id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region 轮播管理
        #region 轮播管理
        public ActionResult BannerList()
        {
            return View();
        }
        public ActionResult BannerAdd()
        {
            return View();
        }
        public ActionResult BannerList_Get()
        {
            var query = entity.lay_banner.OrderBy(p => p.sort);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Banner_Add_Edit(lay_banner bannerModel)
        {
            if (bannerModel.id == 0)
            {
                var query = entity.lay_banner;
                int maxSort = query.Count() <= 0 ? 0 : query.Max(p => p.sort.Value);
                lay_banner banner = new lay_banner()
                {
                    title = bannerModel.title,
                    enable = bannerModel.enable,
                    mode = bannerModel.mode,
                    sort = ++maxSort,
                    page_id = bannerModel.page_id,
                    url = bannerModel.url,
                    path = bannerModel.path,
                    sys_datetime = DateTime.Now
                };
                entity.lay_banner.Add(banner);
            }
            else
            {
                var query = entity.lay_banner.FirstOrDefault(p => p.id == bannerModel.id);
                query.title = bannerModel.title;
                query.enable = bannerModel.enable;
                query.mode = bannerModel.mode;
                query.sort = bannerModel.sort;
                query.page_id = bannerModel.page_id;
                query.url = bannerModel.url;
                query.path = bannerModel.path;
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Banner_Delete(int id)
        {
            var query = entity.lay_banner.FirstOrDefault(p => p.id == id);
            entity.lay_banner.Remove(query);
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Banner_Sort(int id, string sortType)
        {
            var query = entity.lay_banner.OrderBy(p => p.sort).ToArray();
            for (int i = 0; i < query.Count(); i++)
            {
                if (query[i].id == id)
                {
                    if (sortType == "up")
                    {
                        if (i == 0)
                        {
                            break;
                        }
                        else
                        {
                            int tempSort = query[i].sort.Value;
                            query[i].sort = query[i - 1].sort;
                            query[i - 1].sort = tempSort;
                        }
                    }
                    else
                    {
                        if (i == query.Count() - 1)
                        {
                            break;
                        }
                        else
                        {
                            int tempSort = query[i].sort.Value;
                            query[i].sort = query[i + 1].sort;
                            query[i + 1].sort = tempSort;
                        }
                    }
                }
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 轮播内容管理
        public ActionResult BannerContent(int id)
        {
            return View();
        }
        public ActionResult Banner_Get(int id)
        {
            var query = entity.lay_banner.FirstOrDefault(p => p.id == id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region 合作管理
        public ActionResult PartnerList()
        {
            return View();
        }
        public ActionResult PartnerAdd()
        {
            return View();
        }
        public ActionResult PartnerList_Get()
        {
            var query = entity.lay_partner.OrderBy(p => p.sort);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Partner_Add_Edit(lay_partner partnerModel)
        {
            if (partnerModel.id == 0)
            {
                var query = entity.lay_partner;
                int maxSort = query.Count() <= 0 ? 0 : query.Max(p => p.sort.Value);
                lay_partner partner = new lay_partner()
                {
                    title = partnerModel.title,
                    enable = partnerModel.enable,
                    sort = ++maxSort,
                    url = partnerModel.url,
                    path = partnerModel.path,
                    sys_datetime = DateTime.Now
                };
                entity.lay_partner.Add(partner);
            }
            else
            {
                var query = entity.lay_partner.FirstOrDefault(p => p.id == partnerModel.id);
                query.title = partnerModel.title;
                query.enable = partnerModel.enable;
                query.sort = partnerModel.sort;
                query.url = partnerModel.url;
                query.path = partnerModel.path;
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Partner_Delete(int id)
        {
            var query = entity.lay_partner.FirstOrDefault(p => p.id == id);
            entity.lay_partner.Remove(query);
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Partner_Sort(int id, string sortType)
        {
            var query = entity.lay_partner.OrderBy(p => p.sort).ToArray();
            for (int i = 0; i < query.Count(); i++)
            {
                if (query[i].id == id)
                {
                    if (sortType == "up")
                    {
                        if (i == 0)
                        {
                            break;
                        }
                        else
                        {
                            int tempSort = query[i].sort.Value;
                            query[i].sort = query[i - 1].sort;
                            query[i - 1].sort = tempSort;
                        }
                    }
                    else
                    {
                        if (i == query.Count() - 1)
                        {
                            break;
                        }
                        else
                        {
                            int tempSort = query[i].sort.Value;
                            query[i].sort = query[i + 1].sort;
                            query[i + 1].sort = tempSort;
                        }
                    }
                }
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 链接管理
        #region 链接管理
        public ActionResult LinkList()
        {
            return View();
        }
        public ActionResult LinkList_Get()
        {
            var query = entity.lay_link_link.OrderBy(p => p.sort);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Link_Add_Edit(lay_link_link linkModel)
        {
            if (linkModel.id == 0)
            {
                var query = entity.lay_link_link;
                int maxSort = query.Count() <= 0 ? 0 : query.Max(p => p.sort.Value);
                lay_link_link link = new lay_link_link()
                {
                    title = linkModel.title,
                    enable = linkModel.enable,
                    mode = linkModel.mode,
                    sort = ++maxSort,
                    page_id = linkModel.page_id,
                    url = linkModel.url,
                    sys_datetime = DateTime.Now
                };
                entity.lay_link_link.Add(link);
            }
            else
            {
                var query = entity.lay_link_link.FirstOrDefault(p => p.id == linkModel.id);
                query.title = linkModel.title;
                query.enable = linkModel.enable;
                query.mode = linkModel.mode;
                query.sort = linkModel.sort;
                query.page_id = linkModel.page_id;
                query.url = linkModel.url;
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Link_Delete(int id)
        {
            var query = entity.lay_link_link.FirstOrDefault(p => p.id == id);
            entity.lay_link_link.Remove(query);
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Link_Sort(int id, string sortType)
        {
            var query = entity.lay_link_link.OrderBy(p => p.sort).ToArray();
            for (int i = 0; i < query.Count(); i++)
            {
                if (query[i].id == id)
                {
                    if (sortType == "up")
                    {
                        if (i == 0)
                        {
                            break;
                        }
                        else
                        {
                            int tempSort = query[i].sort.Value;
                            query[i].sort = query[i - 1].sort;
                            query[i - 1].sort = tempSort;
                        }
                    }
                    else
                    {
                        if (i == query.Count() - 1)
                        {
                            break;
                        }
                        else
                        {
                            int tempSort = query[i].sort.Value;
                            query[i].sort = query[i + 1].sort;
                            query[i + 1].sort = tempSort;
                        }
                    }
                }
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 链接内容管理
        public ActionResult LinkContent(int id)
        {
            return View();
        }
        public ActionResult Link_Get(int id)
        {
            var query = entity.lay_link_link.FirstOrDefault(p => p.id == id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 子链接管理
        public ActionResult SublinkList_Get(int id)
        {
            var query = entity.lay_link_sublink.Where(p => p.link_id == id).OrderBy(p => p.sort);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Sublink_Add_Edit(lay_link_sublink sublinkModel)
        {
            if (sublinkModel.id == 0)
            {
                var query = entity.lay_link_sublink.Where(p => p.link_id == sublinkModel.link_id);
                int maxSort = query.Count() <= 0 ? 0 : query.Max(p => p.sort.Value);
                lay_link_sublink sublink = new lay_link_sublink()
                {
                    link_id = sublinkModel.link_id,
                    title = sublinkModel.title,
                    enable = sublinkModel.enable,
                    mode = sublinkModel.mode,
                    sort = ++maxSort,
                    page_id = sublinkModel.page_id,
                    url = sublinkModel.url,
                    sys_datetime = DateTime.Now
                };
                entity.lay_link_sublink.Add(sublink);
            }
            else
            {
                var query = entity.lay_link_sublink.FirstOrDefault(p => p.id == sublinkModel.id);
                query.link_id = sublinkModel.link_id;
                query.title = sublinkModel.title;
                query.enable = sublinkModel.enable;
                query.mode = sublinkModel.mode;
                query.sort = sublinkModel.sort;
                query.page_id = sublinkModel.page_id;
                query.url = sublinkModel.url;
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Sublink_Delete(int id)
        {
            var query = entity.lay_link_sublink.FirstOrDefault(p => p.id == id);
            entity.lay_link_sublink.Remove(query);
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Sublink_Sort(int id, string sortType)
        {
            var query = entity.lay_link_sublink.OrderBy(p => p.sort).ToArray();
            for (int i = 0; i < query.Count(); i++)
            {
                if (query[i].id == id)
                {
                    if (sortType == "up")
                    {
                        if (i == 0)
                        {
                            break;
                        }
                        else
                        {
                            int tempSort = query[i].sort.Value;
                            query[i].sort = query[i - 1].sort;
                            query[i - 1].sort = tempSort;
                        }
                    }
                    else
                    {
                        if (i == query.Count() - 1)
                        {
                            break;
                        }
                        else
                        {
                            int tempSort = query[i].sort.Value;
                            query[i].sort = query[i + 1].sort;
                            query[i + 1].sort = tempSort;
                        }
                    }
                }
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 子链接内容管理
        public ActionResult LinksubContent(int id)
        {
            return View();
        }
        public ActionResult Sublink_Get(int id)
        {
            var query = entity.lay_link_sublink.FirstOrDefault(p => p.id == id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region 通知管理
        public ActionResult NoticeList()
        {
            return View();
        }
        public ActionResult NoticeList_Get()
        {
            var query = entity.lay_notice.OrderBy(p => p.sort);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Notice_Add_Edit(lay_notice noticeModel)
        {
            if (noticeModel.id == 0)
            {
                var query = entity.lay_notice;
                int maxSort = query.Count() <= 0 ? 0 : query.Max(p => p.sort.Value);
                lay_notice notice = new lay_notice()
                {
                    enable = noticeModel.enable,
                    sort = ++maxSort,
                    content = noticeModel.content,
                    sys_datetime = DateTime.Now,
                };
                entity.lay_notice.Add(notice);
            }
            else
            {
                var query = entity.lay_notice.FirstOrDefault(p => p.id == noticeModel.id);
                query.enable = noticeModel.enable;
                query.sort = noticeModel.sort;
                query.content = noticeModel.content;
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Notice_Delete(int id)
        {
            var query = entity.lay_notice.FirstOrDefault(p => p.id == id);
            entity.lay_notice.Remove(query);
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Notice_Sort(int id, string sortType)
        {
            var query = entity.lay_notice.OrderBy(p => p.sort).ToArray();
            for (int i = 0; i < query.Count(); i++)
            {
                if (query[i].id == id)
                {
                    if (sortType == "up")
                    {
                        if (i == 0)
                        {
                            break;
                        }
                        else
                        {
                            int tempSort = query[i].sort.Value;
                            query[i].sort = query[i - 1].sort;
                            query[i - 1].sort = tempSort;
                        }
                    }
                    else
                    {
                        if (i == query.Count() - 1)
                        {
                            break;
                        }
                        else
                        {
                            int tempSort = query[i].sort.Value;
                            query[i].sort = query[i + 1].sort;
                            query[i + 1].sort = tempSort;
                        }
                    }
                }
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 网站设置
        public ActionResult Setting()
        {
            return View();
        }
        public ActionResult Setting_Get()
        {
            var query = entity.lay_setting.ToArray();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (var item in query)
            {
                dictionary.Add(item.key, item.value);
            }
            return Json(dictionary, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Setting_Upload(string key, HttpPostedFileBase file)
        {
            string relativePath = "/Upload/setting/";
            string AabsolutePath = Server.MapPath(relativePath);
            string filename = String.Format(key + "-{0}-{1}-{2}-{3}-{4}-{5}-{6}",
                DateTime.Now.Year,
                DateTime.Now.Month.ToString("D2"),
                DateTime.Now.Day.ToString("D2"),
                DateTime.Now.Hour.ToString("D2"),
                DateTime.Now.Minute.ToString("D2"),
                DateTime.Now.Second.ToString("D2"),
                Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName));
            string imgUrl = relativePath + filename;
            if (!Directory.Exists(Path.GetDirectoryName(AabsolutePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(AabsolutePath));
            }
            file.SaveAs(AabsolutePath + filename);
            var query = entity.lay_setting.FirstOrDefault(p => p.key == key);
            if (query == null)
            {
                query = new lay_setting() { key = key, value = null };
                entity.lay_setting.Add(query);
            }
            query.value = imgUrl;
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Setting_Save(string key, string value)
        {
            var query = entity.lay_setting.FirstOrDefault(p => p.key == key);
            if (query == null)
            {
                query = new lay_setting() { key = key, value = null };
                entity.lay_setting.Add(query);
            }
            query.value = value;
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 留言管理
        public ActionResult MessageBoardList()
        {
            return View();
        }
        public ActionResult MessageBoardList_Get()
        {
            var query = entity.module_messageaboard.OrderByDescending(p => p.id).ToArray().Select(p => new
            {
                p.id,
                p.contact_name,
                p.contact_phone,
                p.contact_other,
                p.content,
                p.state_mark,
                p.state_read,
                p.state_solve,
                sys_datetime = p.sys_datetime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            });
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MessageBoard_Add_Edit(module_messageaboard messageaboardModel)
        {
            var query = entity.module_messageaboard.FirstOrDefault(p => p.id == messageaboardModel.id);
            query.state_mark = messageaboardModel.state_mark;
            query.state_read = messageaboardModel.state_read;
            query.state_solve = messageaboardModel.state_solve;
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult MessageBoard_Delete(int id)
        {
            var query = entity.module_messageaboard.FirstOrDefault(p => p.id == id);
            entity.module_messageaboard.Remove(query);
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 招聘管理
        #region 分类管理
        public ActionResult EmployTypeList()
        {
            return View();
        }
        public ActionResult EmployTypeList_Get()
        {
            var query = entity.module_employ_type.OrderBy(p => p.id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmployType_Add_Edit(module_employ_type typeModel)
        {
            if (typeModel.id == 0)
            {
                module_employ_type type = new module_employ_type() { name = typeModel.name };
                entity.module_employ_type.Add(type);
            }
            else
            {
                var query = entity.module_employ_type.FirstOrDefault(p => p.id == typeModel.id);
                query.name = typeModel.name;
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmployType_Delete(int id)
        {
            var query = entity.module_employ_type.FirstOrDefault(p => p.id == id);
            entity.module_employ_type.Remove(query);
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 招聘管理
        public ActionResult EmployList()
        {
            return View();
        }
        public ActionResult EmployAdd()
        {
            return View();
        }
        public ActionResult Employ_Get(int id)
        {
            var query = entity.module_employ.FirstOrDefault(p => p.id == id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EmployList_Get()
        {
            var query = entity.module_employ.OrderBy(p => p.id).Join(entity.module_employ_type, a => a.type_id, b => b.id, (a, b) => new
            {
                a.benefit,
                a.education,
                a.employ_number,
                a.experience,
                a.id,
                a.position_description_1,
                a.position_description_2,
                a.position_description_3,
                a.position_description_4,
                a.position_name,
                a.position_requirement_1,
                a.position_requirement_2,
                a.position_requirement_3,
                a.position_requirement_4,
                a.remark,
                a.salary,
                a.sys_datetime,
                a.type_id,
                a.work_place,
                type = b.name
            });
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Employ_Add_Edit(module_employ employModel)
        {
            if (employModel.id == 0)
            {
                module_employ employ = new module_employ()
                {
                    type_id = employModel.type_id,
                    position_name = employModel.position_name,
                    salary = employModel.salary,
                    education = employModel.education,
                    experience = employModel.experience,
                    work_place = employModel.work_place,
                    employ_number = employModel.employ_number,
                    position_description_1 = employModel.position_description_1,
                    position_description_2 = employModel.position_description_2,
                    position_description_3 = employModel.position_description_3,
                    position_description_4 = employModel.position_description_4,
                    position_requirement_1 = employModel.position_requirement_1,
                    position_requirement_2 = employModel.position_requirement_2,
                    position_requirement_3 = employModel.position_requirement_3,
                    position_requirement_4 = employModel.position_requirement_4,
                    benefit = employModel.benefit,
                    remark = employModel.remark,
                    sys_datetime = DateTime.Now,
                };
                entity.module_employ.Add(employ);
            }
            else
            {
                var query = entity.module_employ.FirstOrDefault(p => p.id == employModel.id);
                query.type_id = employModel.type_id;
                query.position_name = employModel.position_name;
                query.salary = employModel.salary;
                query.education = employModel.education;
                query.experience = employModel.experience;
                query.work_place = employModel.work_place;
                query.employ_number = employModel.employ_number;
                query.position_description_1 = employModel.position_description_1;
                query.position_description_2 = employModel.position_description_2;
                query.position_description_3 = employModel.position_description_3;
                query.position_description_4 = employModel.position_description_4;
                query.position_requirement_1 = employModel.position_requirement_1;
                query.position_requirement_2 = employModel.position_requirement_2;
                query.position_requirement_3 = employModel.position_requirement_3;
                query.position_requirement_4 = employModel.position_requirement_4;
                query.benefit = employModel.benefit;
                query.remark = employModel.remark;
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Employ_Delete(int id)
        {
            var query = entity.module_employ.FirstOrDefault(p => p.id == id);
            entity.module_employ.Remove(query);
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region 新闻管理
        #region 分类管理
        public ActionResult NewsTypeList()
        {
            return View();
        }
        public ActionResult NewsTypeList_Get()
        {
            var query = entity.module_news_type.OrderBy(p => p.id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NewsType_Add_Edit(module_news_type typeModel)
        {
            if (typeModel.id == 0)
            {
                module_news_type type = new module_news_type() { name = typeModel.name };
                entity.module_news_type.Add(type);
            }
            else
            {
                var query = entity.module_news_type.FirstOrDefault(p => p.id == typeModel.id);
                query.name = typeModel.name;
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NewsType_Delete(int id)
        {
            var query = entity.module_news_type.FirstOrDefault(p => p.id == id);
            entity.module_news_type.Remove(query);
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 新闻管理
        public ActionResult NewsList()
        {
            return View();
        }
        public ActionResult NewsAdd()
        {
            return View();
        }
        public ActionResult News_Get(int id)
        {
            var query = entity.module_news.Where(p => p.id == id).ToArray().Select(p => new
            {
                p.author,
                p.content,
                datetime = p.datetime.Value.ToString("yyyy-MM-dd"),
                p.description,
                p.id,
                p.path,
                p.sys_datetime,
                p.title,
                p.top,
                p.type_id,
                p.views,
            }).FirstOrDefault();
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NewsList_Get()
        {
            var query = entity.module_news.OrderBy(p => p.id).ToArray().Join(entity.module_news_type, a => a.type_id, b => b.id, (a, b) => new
            {
                a.author,
                a.content,
                datetime = a.datetime.Value.ToString("yyyy-MM-dd"),
                a.description,
                a.id,
                a.path,
                a.sys_datetime,
                a.title,
                a.top,
                a.type_id,
                a.views,
                type = b.name,
            });
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult News_Add_Edit(module_news newsModel)
        {
            if (newsModel.id == 0)
            {
                module_news news = new module_news()
                {
                    id = 0,
                    author = newsModel.author,
                    content = newsModel.content,
                    datetime = newsModel.datetime,
                    description = newsModel.description,
                    path = newsModel.path,
                    title = newsModel.title,
                    top = newsModel.top,
                    type_id = newsModel.type_id,
                    views = newsModel.views,
                    sys_datetime = DateTime.Now,
                };
                entity.module_news.Add(news);
            }
            else
            {
                var query = entity.module_news.FirstOrDefault(p => p.id == newsModel.id);
                query.author = newsModel.author;
                query.content = newsModel.content;
                query.datetime = newsModel.datetime;
                query.description = newsModel.description;
                query.path = newsModel.path;
                query.title = newsModel.title;
                query.top = newsModel.top;
                query.type_id = newsModel.type_id;
                query.views = newsModel.views;
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult News_Delete(int id)
        {
            var query = entity.module_news.FirstOrDefault(p => p.id == id);
            entity.module_news.Remove(query);
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
        #region 产品管理
        #region 分类管理
        public ActionResult ProductTypeList()
        {
            return View();
        }
        public ActionResult ProductTypeList_Get()
        {
            var query = entity.module_product_type.OrderBy(p => p.id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductType_Add_Edit(module_product_type typeModel)
        {
            if (typeModel.id == 0)
            {
                module_product_type type = new module_product_type() { name = typeModel.name };
                entity.module_product_type.Add(type);
            }
            else
            {
                var query = entity.module_product_type.FirstOrDefault(p => p.id == typeModel.id);
                query.name = typeModel.name;
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductType_Delete(int id)
        {
            var query = entity.module_product_type.FirstOrDefault(p => p.id == id);
            entity.module_product_type.Remove(query);
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 产品管理
        public ActionResult ProductList()
        {
            return View();
        }
        public ActionResult ProductAdd()
        {
            return View();
        }
        public ActionResult Product_Get(int id)
        {
            var query = entity.module_product.FirstOrDefault(p => p.id == id);
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductList_Get()
        {
            var query = entity.module_product.Where(p => p.delete == false).OrderBy(p => p.id).ToArray().Join(entity.module_product_type, a => a.type_id, b => b.id, (a, b) => new
            {
                a.content,
                a.description,
                a.id,
                a.name,
                a.path,
                a.sys_datetime,
                a.top,
                a.type_id,
                a.price,
                a.delete,
                a.attachment,
                type = b.name,
            });
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Product_Add_Edit(module_product productModel)
        {
            if (productModel.id == 0)
            {
                module_product product = new module_product()
                {
                    id = 0,
                    name = productModel.name,
                    content = productModel.content,
                    description = productModel.description,
                    path = productModel.path,
                    top = productModel.top,
                    type_id = productModel.type_id,
                    sys_datetime = DateTime.Now,
                    delete = productModel.delete,
                    price = productModel.price,
                    attachment = productModel.attachment,
                };
                entity.module_product.Add(product);
            }
            else
            {
                var query = entity.module_product.FirstOrDefault(p => p.id == productModel.id);
                query.name = productModel.name;
                query.content = productModel.content;
                query.delete = productModel.delete;
                query.description = productModel.description;
                query.path = productModel.path;
                query.price = productModel.price;
                query.top = productModel.top;
                query.type_id = productModel.type_id;
                query.attachment = productModel.attachment;
            }
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Product_Delete(int id)
        {
            var query = entity.module_product.FirstOrDefault(p => p.id == id);
            query.delete = true;
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region 会员管理
        public ActionResult MemberList()
        {
            return View();
        }
        public ActionResult MemberList_Get()
        {
            var query = entity.account_member.OrderByDescending(p => p.id).ToArray().Select(p => new
            {
                p.id,
                p.phone,
                p.real_name,
                p.idcard_number,
                p.email,
                p.remark,
                p.enable,
                sys_datetime = p.sys_datetime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
            });
            return Json(query, JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        public ActionResult Member_Add_Edit(account_member memberModel)
        {
            var query = entity.account_member.FirstOrDefault(p => p.id == memberModel.id);
            query.enable = memberModel.enable;
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Member_Reset(int id)
        {
            var query = entity.account_member.FirstOrDefault(p => p.id == id);
            query.password = DESTool.Encrypt("1");
            return Json(entity.SaveChanges() > 0, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}