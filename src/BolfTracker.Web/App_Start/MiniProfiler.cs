﻿using System.Linq;
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
            // Be sure to restart you ASP.NET Developement server, this code will not run until you do that. 

            //TODO: See - _MINIPROFILER UPDATED Layout.cshtml
            //      For profiling to display in the UI you will have to include the line @MvcMiniProfiler.MiniProfiler.RenderIncludes() 
            //      in your master layout

            //TODO: Non SQL Server based installs can use other formatters like: new MvcMiniProfiler.SqlFormatters.InlineFormatter()
            MiniProfiler.Settings.SqlFormatter = new MvcMiniProfiler.SqlFormatters.SqlServerFormatter();

            //TODO: To profile a standard DbConnection: 
            // var profiled = new ProfiledDbConnection(cnn, MiniProfiler.Current);

            //TODO: If you are profiling EF code first try: 
            MiniProfilerEF.Initialize();

            //Make sure the MiniProfiler handles BeginRequest and EndRequest
            DynamicModuleUtility.RegisterModule(typeof(MiniProfilerStartupModule));

            //Setup profiler for Controllers via a Global ActionFilter
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
                //TODO: By default only local requests are profiled, optionally you can set it up
                //  so authenticated users are always profiled
                //if (request.IsLocal) { MiniProfiler.Start(); }
                MiniProfiler.Start();
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