using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    [TestClass]
    public class ScoreRepositoryTests : DatabaseTest
    {
        private ShotRepository _scoreRepository;
        private HoleRepository _holeRepository;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _scoreRepository = new ShotRepository(DatabaseFactory, QueryFactory);
            _holeRepository = new HoleRepository(DatabaseFactory, QueryFactory);
            _transaction = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transaction.Dispose();
        }

        [TestMethod]
        public void Should_be_able_to_add_score()
        {
            var score = ObjectMother.CreateScore();

            var hole = ObjectMother.CreateHole(20);
            _holeRepository.Add(hole);
            UnitOfWork.Commit();

            score.Hole = hole;

            _scoreRepository.Add(score);
            UnitOfWork.Commit();

            Assert.AreNotEqual(0, score.Id);
        }
    }
}