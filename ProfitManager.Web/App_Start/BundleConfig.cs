using System.Web.Optimization;

namespace ProfitManager.Web.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new StyleBundle("~/css").Include(
                "~/css/bootstrap.min.css",
                "~/css/TreViewStyle.css"));

            bundles.Add(new StyleBundle("~/css/plugins").Include(
                "~/css/plugins/datatables.bootstrap.min.css",
                "~/css/plugins/codemirror.css",
                "~/css/plugins/dropzone.css",
                "~/css/plugins/fullcalendar.min.css",
                "~/css/plugins/handsontable.full.min.css",
                "~/css/plugins/hover.min.css",
                "~/css/plugins/jquery.steps.css",
                "~/css/plugins/mediaelementplayer.css",
                "~/css/plugins/normalize.css",
                "~/css/plugins/nouislider.min.css",
                "~/css/plugins/select2.min.css",
                "~/css/plugins/spinkit.css",
                "~/css/plugins/typebase.css",
                "~/css/plugins/animate.css",
                "~/css/plugins/font-awesome.min.css",
                "~/css/plugins/animate.min.css",
                "~/css/plugins/simple-line-icons.css",
                "~/css/style.css"));

            bundles.Add(new StyleBundle("~/fonts").Include("~/fonts/fontawesome-webfont.eot",
                "~/fonts/fontawesome-webfont.svg",
                "~/fonts/fontawesome-webfont.ttf",
                "~/fonts/fontawesome-webfont.woff",
                "~/fonts/fontawesome-webfont.woff2",
                "~/fonts/FontAwesome.otf",
                "~/fonts/glyphicons-halflings-regular.eot",
                "~/fonts/glyphicons-halflings-regular.svg",
                "~/fonts/glyphicons-halflings-regular.ttf",
                "~/fonts/glyphicons-halflings-regular.woff",
                "~/fonts/glyphicons-halflings-regular.woff2",
                "~/fonts/Simple-Line-Icons.eot",
                "~/fonts/Simple-Line-Icons.svg",
                "~/fonts/Simple-Line-Icons.ttf",
                "~/fonts/Simple-Line-Icons.woff",
                "~/fonts/Simple-Line-Icons.woff2"));

            bundles.Add(new ScriptBundle("~/Scripts").Include(
                "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/plugins").Include(
                "~/Scripts/plugins/moment.min.js",
                "~/Scripts/vendors.js",
                "~/Scripts/plugins/bootstrap-material-datetimepicker.js",
                "~/Scripts/plugins/jquery.datatables.min.js",
                "~/Scripts/plugins/jquery.steps.min.js",
                "~/Scripts/plugins/datatables.bootstrap.min.js",
                "~/Scripts/plugins/jquery.knob.js",
                "~/Scripts/plugins/jquery.mask.min.js",
                "~/Scripts/plugins/select2.full.min.js",
                "~/Scripts/plugins/jquery.validate.min.js",
                "~/Scripts/plugins/jquery.simply-toast.js",
                "~/Scripts/plugins/morris.min.js",
                "~/Scripts/plugins/raphael.min.js",
                "~/Scripts/plugins/jquery.nicescroll.js",
                "~/Scripts/jquery.tabSlideOut.v1.3.js",
                "~/Scripts/bootstrap-filestyle.js",
                "~/Scripts/main.js"));


            bundles.IgnoreList.Clear();
        }
    }
}