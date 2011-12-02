using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Web.Infrastructure.DynamicModuleHelper;

using MvcMiniProfiler;
using MvcMiniProfiler.MVCHelpers;

[assembly: WebActivator.PreApplicationStartMethod(typeof(BolfTracker.Web.App_Start.MiniProfilerPackage), "PreStart")]
[assembly: WebActivator.PostApplicationStartMethod(typeof(BolfTracker.Web.App_Start.MiniProfilerPackage), "PostStart")]
namespace BolfTracker.Web.App_Start
{
    public class MiniProfilerPackage
    {
        public static void PreStart()
        {
            MiniProfiler.Settings.SqlFormatter = new MvcMiniProfiler.SqlFormatters.SqlServerFormatter();

            MiniProfilerEF.Initialize();

            DynamicModuleUtility.RegisterModule(typeof(MiniProfilerStartupModule));

            GlobalFilters.Filters.Add(new ProfilingActionFilter());
        }

        public static void PostStart()
        {
            // Intercept ViewEngines to profile all partial views and regular views
            var currentViewEngines = ViewEngines.Engines.ToList();

            ViewEngines.Engines.Clear();

            foreach (var viewEngine in currentViewEngines)
            {
                ViewEngines.Engines.Add(new ProfilingViewEngine(viewEngine));
            }
        }
    }

    public class MiniProfilerStartupModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) =>
            {
                var request = ((HttpApplication)sender).Request;

                if (request.IsLocal)
                {
                    MiniProfiler.Start();
                }
            };


            // TODO: You can control who sees the profiling information
            /*
            context.AuthenticateRequest += (sender, e) =>
            {
                if (!CurrentUserIsAllowedToSeeProfiler())
                {
                    MvcMiniProfiler.MiniProfiler.Stop(discardResults: true);
                }
            };
            */

            context.EndRequest += (sender, e) =>
            {
                MiniProfiler.Stop();
            };
        }

        public void Dispose() { }
    }
}
