using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TemplateWeb.Models.DB;

namespace TemplateWeb.Component
{
    public class MemberAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            account_member member = MemberManager.GetMember();
            if (member == null)
            {
                return false;
            }
            else
            {
                if (member.enable == false)
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
            filterContext.Result = new RedirectResult("/Member/Login?redirectUrl="+HttpContext.Current.Request.Url.PathAndQuery);
        }
    }
}