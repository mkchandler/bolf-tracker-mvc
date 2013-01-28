using System.Web.Mvc;

using BolfTracker.Infrastructure.EntityFramework;
using BolfTracker.Repositories;
using BolfTracker.Services;

using Microsoft.Practices.Unity;
using Unity.Mvc3;

namespace BolfTracker.Web
{
    public class UnityConfig
    {
        public static void RegisterUnity()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

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