using System;
using System.Linq;
using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    [TestClass]
    public class HoleRepositoryTests : DatabaseTest
    {
        private HoleRepository _repository;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _repository = new HoleRepository();
            _transaction = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transaction.Dispose();
        }

        [TestMethod]
        public void Should_be_able_to_get_all_holes()
        {
            var initialCount = _repository.All().Count();

            var hole1 = ObjectMother.CreateHole(Int32.MaxValue);
            var hole2 = ObjectMother.CreateHole(Int32.MaxValue - 1);

            _repository.Add(hole1);
            _repository.Add(hole2);
            UnitOfWork.Commit();

            Assert.IsTrue(_repository.All().Count() == initialCount + 2);
        }
    }
}