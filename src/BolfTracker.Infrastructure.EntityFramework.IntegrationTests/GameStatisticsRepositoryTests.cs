using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    [TestClass]
    public class GameStatisticsRepositoryTests
    {
        private GameRepository _gameRepository;
        private GameStatisticsRepository _gameStatisticsRepository;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _gameRepository = new GameRepository();
            _gameStatisticsRepository = new GameStatisticsRepository();
            _transaction = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transaction.Dispose();
        }

        [TestMethod]
        public void Should_be_able_to_add_game_statistics()
        {
            var game = ObjectMother.CreateGame();
            _gameRepository.Add(game);

            var gameStatistics = ObjectMother.CreateGameStatistics(game);
            _gameStatisticsRepository.Add(gameStatistics);

            Assert.AreNotEqual(0, gameStatistics.Id);
        }
    }
}
