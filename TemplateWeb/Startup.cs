using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using TemplateWeb.Component.WxExtension;

[assembly: OwinStartup(typeof(TemplateWeb.Startup))]

namespace TemplateWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=316888

            app.MapSignalR();
        }
    }
}
