using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    [TestClass]
    public class PlayerStatisticsRepositoryTests : DatabaseTest
    {
        private PlayerStatisticsRepository _repository;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new PlayerStatisticsRepository(DatabaseFactory);
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
            var playerStatistics = ObjectMother.CreatePlayerStatistics();

            _repository.Add(playerStatistics);
            UnitOfWork.Commit();

            Assert.AreNotEqual(0, playerStatistics.Id);
        }
    }
}