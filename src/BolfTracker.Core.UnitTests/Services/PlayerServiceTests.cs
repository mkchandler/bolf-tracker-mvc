using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        private Mock<IUnitOfWork> _unitOfWork;

        private PlayerService _playerService;

        [TestInitialize]
        public void TestInitialize()
        {
            _playerRepository = new Mock<IPlayerRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();

            _playerService = new PlayerService(_playerRepository.Object, _unitOfWork.Object);
        }

        [TestMethod]
        public void Should_be_able_to_get_all_players()
        {

        }
    }
}