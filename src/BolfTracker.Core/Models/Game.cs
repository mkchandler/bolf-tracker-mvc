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

        public Game()
        {
            _shots = new List<Shot>();
            _gameStatistics = new List<GameStatistics>();
            _playerGameStatistics = new List<PlayerGameStatistics>();

            Date = DateTime.Now;
        }

        public virtual int Id
        {
            get;
            set;
        }

        [Required]
        public virtual DateTime Date
        {
            get;
            set;
        }

        public virtual ICollection<Shot> Shots
        {
            get { return _shots; }
        }

        public virtual ICollection<GameStatistics> GameStatistics
        {
            get { return _gameStatistics; }
        }

        public virtual ICollection<PlayerGameStatistics> PlayerGameStatistics
        {
            get { return _playerGameStatistics; }
        }
    }
}