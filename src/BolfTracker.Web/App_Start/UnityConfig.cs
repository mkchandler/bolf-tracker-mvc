using System;
using BolfTracker.Infrastructure.EntityFramework;
using BolfTracker.Repositories;
using BolfTracker.Services;
using Microsoft.Practices.Unity;

namespace BolfTracker.Web.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
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
        }
    }
}
