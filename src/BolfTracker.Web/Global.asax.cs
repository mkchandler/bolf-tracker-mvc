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

            routes.MapRoute("Login", "login", new { controller = "Player", action = "Login" });
            routes.MapRoute("SignOut", "signout", new { controller = "Player", action = "SignOut" });

            routes.MapRoute("Rankings", "rankings", new { controller = "Ranking", action = "Index" });
            routes.MapRoute("RankingsByMonth", "rankings/{year}/{month}", new { controller = "Ranking", action = "Index", year = UrlParameter.Optional, month = UrlParameter.Optional });

            routes.MapRoute("Games", "games", new { controller = "Game", action = "Index" });
            routes.MapRoute("GamesByMonth", "games/{year}/{month}", new { controller = "Game", action = "Index", year = UrlParameter.Optional, month = UrlParameter.Optional });
            routes.MapRoute("CreateGame", "games/create", new { controller = "Game", action = "Create" });
            routes.MapRoute("GameDetails", "games/{id}", new { controller = "Game", action = "Details" });

            routes.MapRoute("Holes", "holes", new { controller = "Hole", action = "Index" });
            routes.MapRoute("HoleDetails", "holes/{id}", new { controller = "Hole", action = "Details" });

            routes.MapRoute("Players", "players", new { controller = "Player", action = "Index" });
            routes.MapRoute("PlayerDetails", "players/{id}/{name}", new { controller = "Player", action = "Details", name = UrlParameter.Optional });

            routes.MapRoute("Statistics", "stats", new { controller = "Statistics", action = "Index" });

            routes.MapRoute("Landing", "", new { controller = "Ranking", action = "Index" });

            routes.MapRoute("Default", "{controller}/{action}", new { controller = "Home", action = "Index" });
        }

        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

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
                     .RegisterType<IHoleStatisticsRepository, HoleStatisticsRepository>(new HttpContextLifetimeManager<IHoleStatisticsRepository>())
                     .RegisterType<IPlayerHoleStatisticsRepository, PlayerHoleStatisticsRepository>(new HttpContextLifetimeManager<IPlayerHoleStatisticsRepository>())
                     .RegisterType<IPlayerGameStatisticsRepository, PlayerGameStatisticsRepository>(new HttpContextLifetimeManager<IPlayerGameStatisticsRepository>())
                     .RegisterType<IPlayerCareerStatisticsRepository, PlayerCareerStatisticsRepository>(new HttpContextLifetimeManager<IPlayerCareerStatisticsRepository>())
                     .RegisterType<IShotTypeRepository, ShotTypeRepository>(new HttpContextLifetimeManager<IShotTypeRepository>());

            // Set up and register the database provider
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["BolfTracker"];
            DbProviderFactory databaseProviderFactory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);

            container.RegisterInstance(databaseProviderFactory);
            container.RegisterType<IDatabaseFactory, DatabaseFactory>(new HttpContextLifetimeManager<IDatabaseFactory>(), new InjectionConstructor(typeof(DbProviderFactory), connectionStringSettings.ConnectionString));

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