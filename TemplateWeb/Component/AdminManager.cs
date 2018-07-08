using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TemplateWeb.Models.DB;

namespace TemplateWeb.Component
{
    public class AdminManager
    {
        
        public static account_admin GetAdmin()
        {
            var session = HttpContext.Current.Session["tpadmin"];
            if (session == null)
            {
                return null;
            }
            return (account_admin)session;
        }
    }
}