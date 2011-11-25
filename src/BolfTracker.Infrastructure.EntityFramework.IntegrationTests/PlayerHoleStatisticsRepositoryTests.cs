using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    [TestClass]
    public class PlayerHoleStatisticsRepositoryTests : DatabaseTest
    {
        private PlayerHoleStatisticsRepository _repository;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new PlayerHoleStatisticsRepository(DatabaseFactory);
            _transaction = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transaction.Dispose();
        }

        [TestMethod]
        public void Should_be_able_to_add_player_hole_statistics()
        {
            var playerHoleStatistics = ObjectMother.CreatePlayerHoleStatistics();

            _repository.Add(playerHoleStatistics);
            UnitOfWork.Commit();

            Assert.AreNotEqual(0, playerHoleStatistics.Id);
        }
    }
}