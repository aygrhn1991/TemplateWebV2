using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TemplateWeb.Models.DB;

namespace TemplateWeb.Component
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            account_admin admin = AdminManager.GetAdmin();
            if (admin == null)
            {
                return false;
            }
            else
            {
                if (admin.enable == false)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Admin/Login");
        }
    }
}