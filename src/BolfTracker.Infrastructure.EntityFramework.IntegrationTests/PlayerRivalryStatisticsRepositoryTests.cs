using System;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    [TestClass]
    public class PlayerRivalryStatisticsRepositoryTests
    {
        private GameRepository _gameRepository;
        private HoleRepository _holeRepository;
        private PlayerRepository _playerRepository;
        private ShotTypeRepository _shotTypeRepository;
        private PlayerRivalryStatisticsRepository _playerRivalryStatisticsRepository;
        private TransactionScope _transaction;

        [TestInitialize]
        public void Initialize()
        {
            _gameRepository = new GameRepository();
            _holeRepository = new HoleRepository();
            _playerRepository = new PlayerRepository();
            _shotTypeRepository = new ShotTypeRepository();
            _playerRivalryStatisticsRepository = new PlayerRivalryStatisticsRepository();
            _transaction = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _transaction.Dispose();
        }

        [TestMethod]
        public void Should_be_able_to_add_player_rivalry_statistics()
        {
            var game = ObjectMother.CreateGame();
            _gameRepository.Add(game);

            var player = ObjectMother.CreatePlayer();
            var affectedPlayer = ObjectMother.CreatePlayer();
            _playerRepository.Add(player);
            _playerRepository.Add(affectedPlayer);

            var hole = ObjectMother.CreateHole(Int32.MaxValue);
            _holeRepository.Add(hole);

            var shotType = ObjectMother.CreateShotType();
            _shotTypeRepository.Add(shotType);

            var playerRivalryStatistics = ObjectMother.CreatePlayerRivalryStatistics(game, player, affectedPlayer, hole, shotType);
            _playerRivalryStatisticsRepository.Add(playerRivalryStatistics);

            Assert.AreNotEqual(0, playerRivalryStatistics.Id);
        }
    }
}
