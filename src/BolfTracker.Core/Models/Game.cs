using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BolfTracker.Models
{
    public class Game : IEntity
    {
        private readonly ICollection<Shot> _shots;
        private readonly ICollection<GameStatistics> _gameStatistics;
        private readonly ICollection<PlayerGameStatistics> _playerGameStatistics;
        private readonly ICollection<PlayerRivalryStatistics> _playerRivalryStatistics;
        private readonly ICollection<PlayerBadges> _playerBadges;
        public Game()
        {
            _shots = new List<Shot>();
            _gameStatistics = new List<GameStatistics>();
            _playerGameStatistics = new List<PlayerGameStatistics>();
            _playerRivalryStatistics = new List<PlayerRivalryStatistics>();
            _playerBadges = new List<PlayerBadges>();
            Date = DateTime.Now;
        }

        public int Id
        {
            get;
            set;
        }

        [Required]
        public DateTime Date
        {
            get;
            set;
        }

        public ICollection<Shot> Shots
        {
            get { return _shots; }
        }

        public ICollection<GameStatistics> GameStatistics
        {
            get { return _gameStatistics; }
        }

        public ICollection<PlayerGameStatistics> PlayerGameStatistics
        {
            get { return _playerGameStatistics; }
        }

        public ICollection<PlayerRivalryStatistics> PlayerRivalryStatistics
        {
            get { return _playerRivalryStatistics; }
        }

        public ICollection<PlayerBadges> PlayerBadges
        {
            get { return _playerBadges; }
        }
    }
}
