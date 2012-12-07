using System;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    [TestClass]
    public class HoleStatisticsRepositoryTests
    {
        private HoleRepository _holeRepository;
        private HoleStatisticsRepository _holeStatisticsRepository;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _holeRepository = new HoleRepository();
            _holeStatisticsRepository = new HoleStatisticsRepository();
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
            var hole = ObjectMother.CreateHole(Int32.MaxValue);
            _holeRepository.Add(hole);

            var holeStatistics = ObjectMother.CreateHoleStatistics(hole);
            _holeStatisticsRepository.Add(holeStatistics);

            Assert.AreNotEqual(0, holeStatistics.Id);
        }
    }
}
