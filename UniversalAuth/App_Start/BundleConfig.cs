using System.Web;
using System.Web.Optimization;

namespace UniversalAuth
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Res/js").Include(                        
                        "~/Res/Libs/jquery/jquery-1.8.2.min.js",
                        "~/Res/Libs/layer-v3.1.1/layer.js",
                        "~/Res/Libs/layui-v2.4.3/layui.js",
                        "~/Res/Js/Shared/_Layout.js"));

            bundles.Add(new StyleBundle("~/Res/css").Include(
                      "~/Res/Libs/Font-Awesome-3.2.1/css/font-awesome.min.css",
                      "~/Res/Libs/layui-v2.4.3/css/layui.css",
                      "~/Res/Css/Shared/_Layout.css"));
        }
    }
}
