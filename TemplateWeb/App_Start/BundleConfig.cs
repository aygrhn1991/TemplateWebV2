using System.Web;
using System.Web.Optimization;

namespace TemplateWeb
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css-lib").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css"));

            bundles.Add(new ScriptBundle("~/bundles/script-lib").Include(
                      "~/Scripts/jquery-2.2.4.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/angular.js"));
        }
    }
}