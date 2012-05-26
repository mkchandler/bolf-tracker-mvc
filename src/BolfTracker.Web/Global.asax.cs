using System.Web.Mvc;
using System.Web.Routing;

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

            routes.MapRoute("LeagueManagerStatistics", "leaguemanager", new { controller = "Admin", action = "Index" });
            routes.MapRoute("LeagueManagerPlayers", "leaguemanager/players", new { controller = "Admin", action = "Players" });
            routes.MapRoute("LeagueManagerHoles", "leaguemanager/holes", new { controller = "Admin", action = "Holes" });

            routes.MapRoute("Rankings", "rankings", new { controller = "Ranking", action = "Index" });
            routes.MapRoute("RankingsByMonth", "rankings/{year}/{month}", new { controller = "Ranking", action = "Index", year = UrlParameter.Optional, month = UrlParameter.Optional });

            routes.MapRoute("CreateGame", "games/create", new { controller = "Game", action = "Create" });
            routes.MapRoute("GameDetails", "games/{id}", new { controller = "Game", action = "Details" });
            routes.MapRoute("Games", "games", new { controller = "Game", action = "Index" });
            routes.MapRoute("GamesByMonth", "games/{year}/{month}", new { controller = "Game", action = "Index", year = UrlParameter.Optional, month = UrlParameter.Optional });

            routes.MapRoute("Holes", "holes", new { controller = "Hole", action = "Index" });
            routes.MapRoute("HoleDetails", "holes/{id}", new { controller = "Hole", action = "Details" });

            routes.MapRoute("Players", "players", new { controller = "Player", action = "Index" });
            routes.MapRoute("PlayerDetails", "players/{id}/{name}", new { controller = "Player", action = "Details", name = UrlParameter.Optional });

            routes.MapRoute("Statistics", "stats", new { controller = "Statistics", action = "Index" });

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

        private IUnityContainer GetUnityContainer()
        {
            IUnityContainer container = new UnityContainer();

            // Register all of the needed services
            container.RegisterType<IPlayerService, PlayerService>()
                     .RegisterType<IGameService, GameService>()
                     .RegisterType<IHoleService, HoleService>()
                     .RegisterType<IShotService, ShotService>()
                     .RegisterType<IShotTypeService, ShotTypeService>()
                     .RegisterType<IRankingService, RankingService>();

            // Register all of the needed repositories
            container.RegisterType<IGameRepository, GameRepository>()
                     .RegisterType<IPlayerRepository, PlayerRepository>()
                     .RegisterType<IHoleRepository, HoleRepository>()
                     .RegisterType<IShotRepository, ShotRepository>()
                     .RegisterType<IRankingRepository, RankingRepository>()
                     .RegisterType<IGameStatisticsRepository, GameStatisticsRepository>()
                     .RegisterType<IPlayerStatisticsRepository, PlayerStatisticsRepository>()
                     .RegisterType<IHoleStatisticsRepository, HoleStatisticsRepository>()
                     .RegisterType<IPlayerHoleStatisticsRepository, PlayerHoleStatisticsRepository>()
                     .RegisterType<IPlayerGameStatisticsRepository, PlayerGameStatisticsRepository>()
                     .RegisterType<IPlayerCareerStatisticsRepository, PlayerCareerStatisticsRepository>()
                     .RegisterType<IPlayerRivalryStatisticsRepository, PlayerRivalryStatisticsRepository>()
                     .RegisterType<IShotTypeRepository, ShotTypeRepository>();

            return container;
        }
    }
}
