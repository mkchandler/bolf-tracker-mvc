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
        public void ShouldBeAbleToAddGameStatistics()
        {
            var game = ObjectMother.CreateGame();
            _gameRepository.Add(game);

            var gameStatistics = ObjectMother.CreateGameStatistics(game);
            _gameStatisticsRepository.Add(gameStatistics);

            Assert.AreNotEqual(0, gameStatistics.Id);
        }

        [TestMethod]
        public void ShouldBeAbleToDeleteGameStatistics()
        {
            var game = ObjectMother.CreateGame();
            _gameRepository.Add(game);

            var gameStatistics = ObjectMother.CreateGameStatistics(game);
            _gameStatisticsRepository.Add(gameStatistics);
            _gameStatisticsRepository.Delete(gameStatistics);

            Assert.IsNotNull(_gameRepository.GetById(game.Id));
            Assert.IsNull(_gameStatisticsRepository.GetById(gameStatistics.Id));
        }
    }
}
