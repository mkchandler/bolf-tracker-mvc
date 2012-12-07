using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    [TestClass]
    public class RankingRepositoryTests
    {
        private PlayerRepository _playerRepository;
        private RankingRepository _rankingRepository;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _playerRepository = new PlayerRepository();
            _rankingRepository = new RankingRepository();
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
            var player = ObjectMother.CreatePlayer();
            _playerRepository.Add(player);

            var ranking = ObjectMother.CreateRanking(player);
            _rankingRepository.Add(ranking);

            Assert.AreNotEqual(0, ranking.Id);
        }
    }
}
