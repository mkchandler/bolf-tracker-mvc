using System.Web;
using System.Web.Optimization;

namespace BolfTracker.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css")
                   .Include("~/css/bootstrap.css")
                   .Include("~/css/bolftracker.css"));
        }
    }
}
