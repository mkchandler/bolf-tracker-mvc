using System;
using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    [TestClass]
    public class ShotRepositoryTests
    {
        private GameRepository _gameRepository;
        private HoleRepository _holeRepository;
        private PlayerRepository _playerRepository;
        private ShotTypeRepository _shotTypeRepository;
        private ShotRepository _shotRepository;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _gameRepository = new GameRepository();
            _holeRepository = new HoleRepository();
            _playerRepository = new PlayerRepository();
            _shotTypeRepository = new ShotTypeRepository();
            _shotRepository = new ShotRepository();
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
            var game = ObjectMother.CreateGame();
            _gameRepository.Add(game);

            var player = ObjectMother.CreatePlayer();
            _playerRepository.Add(player);

            var shotType = ObjectMother.CreateShotType();
            _shotTypeRepository.Add(shotType);

            var hole = ObjectMother.CreateHole(Int32.MaxValue);
            _holeRepository.Add(hole);

            var shot = ObjectMother.CreateShot(game, player, shotType, hole);
            _shotRepository.Add(shot);

            Assert.AreNotEqual(0, shot.Id);
        }
    }
}
