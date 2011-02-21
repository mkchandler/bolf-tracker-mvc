using BolfTracker.Infrastructure;
using BolfTracker.Repositories;
using BolfTracker.Services;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace BolfTracker.UnitTests.Services
{
    [TestClass]
    public class PlayerServiceTests
    {
        private const string PlayerName = "Furia Roja";

        private Mock<IPlayerRepository> _playerRepository;
        private Mock<IPlayerStatisticsRepository> _playerStatisticsRepository;
        private Mock<IPlayerHoleStatisticsRepository> _playerHoleStatisticsRepository;
        private Mock<IGameStatisticsRepository> _gameStatisticsRepository;
        private Mock<IUnitOfWork> _unitOfWork;

        private PlayerService _playerService;

        [TestInitialize]
        public void TestInitialize()
        {
            _playerRepository = new Mock<IPlayerRepository>();
            _playerStatisticsRepository = new Mock<IPlayerStatisticsRepository>();
            _playerHoleStatisticsRepository = new Mock<IPlayerHoleStatisticsRepository>();
            _gameStatisticsRepository = new Mock<IGameStatisticsRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();

            _playerService = new PlayerService(_playerRepository.Object, _playerStatisticsRepository.Object, _playerHoleStatisticsRepository.Object, _gameStatisticsRepository.Object, _unitOfWork.Object);
        }

        [TestMethod]
        public void Should_be_able_to_get_all_players()
        {

        }
    }
}