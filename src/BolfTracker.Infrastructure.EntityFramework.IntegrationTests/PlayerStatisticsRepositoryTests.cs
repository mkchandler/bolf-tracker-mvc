using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    [TestClass]
    public class PlayerStatisticsRepositoryTests
    {
        private PlayerRepository _playerRepository;
        private PlayerStatisticsRepository _playerStatisticsRepository;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _playerRepository = new PlayerRepository();
            _playerStatisticsRepository = new PlayerStatisticsRepository();
            _transaction = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transaction.Dispose();
        }

        [TestMethod]
        public void Should_be_able_to_add_player_statistics()
        {
            var player = ObjectMother.CreatePlayer();
            _playerRepository.Add(player);

            var playerStatistics = ObjectMother.CreatePlayerStatistics(player);
            _playerStatisticsRepository.Add(playerStatistics);

            Assert.AreNotEqual(0, playerStatistics.Id);
        }
    }
}
