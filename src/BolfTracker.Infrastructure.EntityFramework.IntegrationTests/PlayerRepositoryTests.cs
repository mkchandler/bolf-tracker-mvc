using System.Linq;
using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    [TestClass]
    public class PlayerRepositoryTests
    {
        private PlayerRepository _repository;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new PlayerRepository();
            _transaction = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transaction.Dispose();
        }

        [TestMethod]
        public void Should_be_able_to_add_player()
        {
            var player = ObjectMother.CreatePlayer();

            _repository.Add(player);

            Assert.AreNotEqual(0, player.Id);
        }

        [TestMethod]
        public void Should_be_able_to_get_all_players()
        {
            var initialCount = _repository.All().Count();

            var player1 = ObjectMother.CreatePlayer();
            var player2 = ObjectMother.CreatePlayer();

            _repository.Add(player1);
            _repository.Add(player2);

            Assert.IsTrue(_repository.All().Count() == initialCount + 2);
        }

        [TestMethod]
        public void Should_be_able_to_get_player_by_id()
        {
            var player = ObjectMother.CreatePlayer();

            _repository.Add(player);

            Assert.IsNotNull(_repository.GetById(player.Id));
        }


        [TestMethod]
        public void Should_be_able_to_get_player_by_name()
        {
            var player = ObjectMother.CreatePlayer();

            _repository.Add(player);

            Assert.IsNotNull(_repository.GetByName(player.Name));
        }
    }
}
