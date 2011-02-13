using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    [TestClass]
    public class GameRepositoryTests : DatabaseTest
    {
        private GameRepository _repository;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new GameRepository(DatabaseFactory, QueryFactory);
            _transaction = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transaction.Dispose();
        }

        [TestMethod]
        public void Should_be_able_to_add_game()
        {
            var game = ObjectMother.CreateGame();

            _repository.Add(game);
            UnitOfWork.Commit();

            Assert.AreNotEqual(0, game.Id);
        }
    }
}