using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    [TestClass]
    public class GameStatisticsRepositoryTests : DatabaseTest
    {
        private GameStatisticsRepository _repository;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new GameStatisticsRepository(DatabaseFactory, QueryFactory);
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
            var gameStatistics = ObjectMother.CreateGameStatistics();

            _repository.Add(gameStatistics);
            UnitOfWork.Commit();

            Assert.AreNotEqual(0, gameStatistics.Id);
        }
    }
}