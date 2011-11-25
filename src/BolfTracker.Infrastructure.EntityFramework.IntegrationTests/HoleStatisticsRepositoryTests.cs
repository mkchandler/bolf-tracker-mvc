using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    [TestClass]
    public class HoleStatisticsRepositoryTests : DatabaseTest
    {
        private HoleStatisticsRepository _repository;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new HoleStatisticsRepository(DatabaseFactory);
            _transaction = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transaction.Dispose();
        }

        [TestMethod]
        public void Should_be_able_to_add_hole_statistics()
        {
            var holeStatistics = ObjectMother.CreateHoleStatistics();

            _repository.Add(holeStatistics);
            UnitOfWork.Commit();

            Assert.AreNotEqual(0, holeStatistics.Id);
        }
    }
}