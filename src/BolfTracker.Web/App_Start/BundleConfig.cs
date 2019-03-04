using System.Web.Optimization;

namespace BolfTracker.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js")
                   .Include("~/scripts/jquery-3.3.1.js")
                   .Include("~/scripts/bootstrap.js")
                   .Include("~/scripts/jquery.tablesorter.js"));

            bundles.Add(new StyleBundle("~/bundles/css")
                   .Include("~/content/bootstrap.css")
                   .Include("~/content/bolftracker.css"));
        }
    }
}
