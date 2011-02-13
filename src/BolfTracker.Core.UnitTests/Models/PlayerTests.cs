using System.Linq;

using BolfTracker.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BolfTracker.UnitTests.Models
{
    [TestClass]
    public class PlayerTests
    {
        private Player _player;

        [TestInitialize]
        public void PlayerTestsInitialize()
        {
            _player = new Player();
        }

        [TestMethod]
        public void Scores_should_be_empty_when_new_instance_is_created()
        {
            Assert.IsFalse(_player.Shots.Any());
        }
    }
}