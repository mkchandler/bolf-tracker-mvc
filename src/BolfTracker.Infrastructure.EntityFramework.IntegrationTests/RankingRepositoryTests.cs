using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    [TestClass]
    public class RankingRepositoryTests : DatabaseTest
    {
        private RankingRepository _repository;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new RankingRepository(DatabaseFactory);
            _transaction = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transaction.Dispose();
        }

        [TestMethod]
        public void Should_be_able_to_add_ranking()
        {
            var ranking = ObjectMother.CreateRanking();

            _repository.Add(ranking);
            UnitOfWork.Commit();

            Assert.AreNotEqual(0, ranking.Id);
        }
    }
}
