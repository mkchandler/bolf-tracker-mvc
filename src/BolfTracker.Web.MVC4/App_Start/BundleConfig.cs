using System.Web;
using System.Web.Optimization;

namespace BolfTracker.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js")
                   .Include("~/js/jquery-1.9.1.js")
                   .Include("~/js/bootstrap.js")
                   .Include("~/js/jquery.tablesorter.js"));

            bundles.Add(new StyleBundle("~/bundles/css")
                   .Include("~/css/bootstrap.css")
                   .Include("~/css/bolftracker.css"));
        }
    }
}
