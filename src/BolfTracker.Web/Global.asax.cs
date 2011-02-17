using System;
using System.Configuration;
using System.Data.Common;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using BolfTracker.Infrastructure;
using BolfTracker.Infrastructure.EntityFramework;
using BolfTracker.Repositories;
using BolfTracker.Services;

using Microsoft.Practices.Unity;

namespace BolfTracker.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Rankings", "rankings", new { controller = "Ranking", action = "Index" });

            routes.MapRoute("Games", "games", new { controller = "Game", action = "Index" });
            routes.MapRoute("GamePanel", "games/{id}", new { controller = "Game", action = "Details" });

            routes.MapRoute("Holes", "holes", new { controller = "Hole", action = "Index" });

            routes.MapRoute("Players", "players", new { controller = "Player", action = "Index" });

            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Ranking", action = "Index", id = UrlParameter.Optional });
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            IUnityContainer container = GetUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static readonly Func<LifetimeManager> transient = () => new TransientLifetimeManager();
        private static readonly Func<LifetimeManager> singleton = () => new ContainerControlledLifetimeManager();

        private IUnityContainer GetUnityContainer()
        {
            IUnityContainer container = new UnityContainer();

            // Register all of the needed services
            container.RegisterType<IPlayerService, PlayerService>(new HttpContextLifetimeManager<IPlayerService>())
                     .RegisterType<IGameService, GameService>(new HttpContextLifetimeManager<IGameService>())
                     .RegisterType<IHoleService, HoleService>(new HttpContextLifetimeManager<IHoleService>())
                     .RegisterType<IShotService, ShotService>(new HttpContextLifetimeManager<IShotService>())
                     .RegisterType<IShotTypeService, ShotTypeService>(new HttpContextLifetimeManager<IShotTypeService>())
                     .RegisterType<IRankingService, RankingService>(new HttpContextLifetimeManager<IRankingService>())
                     .RegisterType<IUnitOfWork, UnitOfWork>(new HttpContextLifetimeManager<IUnitOfWork>());

            // Register all of the needed repositories
            container.RegisterType<IGameRepository, GameRepository>(new HttpContextLifetimeManager<IGameRepository>())
                     .RegisterType<IPlayerRepository, PlayerRepository>(new HttpContextLifetimeManager<IPlayerRepository>())
                     .RegisterType<IHoleRepository, HoleRepository>(new HttpContextLifetimeManager<IHoleRepository>())
                     .RegisterType<IShotRepository, ShotRepository>(new HttpContextLifetimeManager<IShotRepository>())
                     .RegisterType<IRankingRepository, RankingRepository>(new HttpContextLifetimeManager<IRankingRepository>())
                     .RegisterType<IGameStatisticsRepository, GameStatisticsRepository>(new HttpContextLifetimeManager<IGameStatisticsRepository>())
                     .RegisterType<IPlayerStatisticsRepository, PlayerStatisticsRepository>(new HttpContextLifetimeManager<IPlayerStatisticsRepository>())
                     .RegisterType<IShotTypeRepository, ShotTypeRepository>(new HttpContextLifetimeManager<IShotTypeRepository>());

            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["BolfTracker"];

            string providerName = connectionStringSettings.ProviderName;

            DbProviderFactory databaseProviderFactory = DbProviderFactories.GetFactory(providerName);
            container.RegisterInstance(databaseProviderFactory);

            string connectionString = connectionStringSettings.ConnectionString;
            bool? useCompliledQuery = null;
            bool temp;

            if (Boolean.TryParse(ConfigurationManager.AppSettings["UseCompliedQuery"], out temp))
            {
                useCompliledQuery = temp;
            }

            container.RegisterType<IDatabaseFactory, DatabaseFactory>(new HttpContextLifetimeManager<IDatabaseFactory>(), new InjectionConstructor(typeof(DbProviderFactory), connectionString))
                     .RegisterType<IQueryFactory, QueryFactory>(new HttpContextLifetimeManager<IQueryFactory>(), new InjectionConstructor(false, useCompliledQuery ?? true));

            return container;
        }
    }

    public class HttpContextLifetimeManager<T> : LifetimeManager, IDisposable
    {
        public override object GetValue()
        {
            return HttpContext.Current.Items[typeof(T).AssemblyQualifiedName];
        }

        public override void RemoveValue()
        {
            HttpContext.Current.Items.Remove(typeof(T).AssemblyQualifiedName);
        }

        public override void SetValue(object newValue)
        {
            HttpContext.Current.Items[typeof(T).AssemblyQualifiedName] = newValue;
        }

        public void Dispose()
        {
            RemoveValue();
        }
    }
}