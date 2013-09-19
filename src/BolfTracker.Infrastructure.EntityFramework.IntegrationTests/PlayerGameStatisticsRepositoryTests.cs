using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    [TestClass]
    public class PlayerGameStatisticsRepositoryTests
    {
        private GameRepository _gameRepository;
        private PlayerRepository _playerRepository;
        private PlayerGameStatisticsRepository _playerGameStatisticsRepository;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _gameRepository = new GameRepository();
            _playerRepository = new PlayerRepository();
            _playerGameStatisticsRepository = new PlayerGameStatisticsRepository();
            _transaction = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transaction.Dispose();
        }

        [TestMethod]
        public void ShouldBeAbleToAddPlayerGameStatistics()
        {
            var game = ObjectMother.CreateGame();
            _gameRepository.Add(game);

            var player = ObjectMother.CreatePlayer();
            _playerRepository.Add(player);

            var playerGameStatistics = ObjectMother.CreatePlayerGameStatistics(game, player);
            _playerGameStatisticsRepository.Add(playerGameStatistics);

            Assert.AreNotEqual(0, playerGameStatistics.Id);
        }

        [TestMethod]
        public void ShouldBeAbleToDeletePlayerGameStatistics()
        {
            var game = ObjectMother.CreateGame();
            _gameRepository.Add(game);

            var player = ObjectMother.CreatePlayer();
            _playerRepository.Add(player);

            var playerGameStatistics = ObjectMother.CreatePlayerGameStatistics(game, player);
            _playerGameStatisticsRepository.Add(playerGameStatistics);
            _playerGameStatisticsRepository.Delete(playerGameStatistics);

            Assert.IsNotNull(_gameRepository.GetById(game.Id));
            Assert.IsNull(_playerGameStatisticsRepository.GetById(playerGameStatistics.Id));
        }
    }
}
