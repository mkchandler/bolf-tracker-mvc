using System.Web.Mvc;
using System.Web.Routing;

namespace BolfTracker.Web
{
    public class RouteConfig
    {
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
    }
}
