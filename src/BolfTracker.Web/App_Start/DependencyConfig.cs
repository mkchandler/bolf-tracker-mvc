using System.Reflection;
using System.Web.Mvc;
using BolfTracker.Infrastructure.EntityFramework;
using BolfTracker.Repositories;
using BolfTracker.Services;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace BolfTracker.Web
{
    public class DependencyConfig
    {
        public static void RegisterDependencies()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            // Register all of the services
            container.Register<IPlayerService, PlayerService>(Lifestyle.Scoped);
            container.Register<IGameService, GameService>(Lifestyle.Scoped);
            container.Register<IHoleService, HoleService>(Lifestyle.Scoped);
            container.Register<IShotService, ShotService>(Lifestyle.Scoped);
            container.Register<IShotTypeService, ShotTypeService>(Lifestyle.Scoped);
            container.Register<IRankingService, RankingService>(Lifestyle.Scoped);

            // Register all of the repositories
            container.Register<IGameRepository, GameRepository>(Lifestyle.Scoped);
            container.Register<IPlayerRepository, PlayerRepository>(Lifestyle.Scoped);
            container.Register<IHoleRepository, HoleRepository>(Lifestyle.Scoped);
            container.Register<IShotRepository, ShotRepository>(Lifestyle.Scoped);
            container.Register<IRankingRepository, RankingRepository>(Lifestyle.Scoped);
            container.Register<IGameStatisticsRepository, GameStatisticsRepository>(Lifestyle.Scoped);
            container.Register<IPlayerStatisticsRepository, PlayerStatisticsRepository>(Lifestyle.Scoped);
            container.Register<IHoleStatisticsRepository, HoleStatisticsRepository>(Lifestyle.Scoped);
            container.Register<IPlayerHoleStatisticsRepository, PlayerHoleStatisticsRepository>(Lifestyle.Scoped);
            container.Register<IPlayerGameStatisticsRepository, PlayerGameStatisticsRepository>(Lifestyle.Scoped);
            container.Register<IPlayerCareerStatisticsRepository, PlayerCareerStatisticsRepository>(Lifestyle.Scoped);
            container.Register<IPlayerRivalryStatisticsRepository, PlayerRivalryStatisticsRepository>(Lifestyle.Scoped);
            container.Register<IShotTypeRepository, ShotTypeRepository>(Lifestyle.Scoped);

            // This is an extension method from the integration package
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}
