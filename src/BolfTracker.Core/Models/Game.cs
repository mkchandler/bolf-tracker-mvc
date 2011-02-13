using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BolfTracker.Models
{
    public class Game : IEntity
    {
        private readonly ICollection<Shot> _shots;
        private readonly ICollection<GameStatistics> _gameStatistics;

        public Game()
        {
            _shots = new List<Shot>();
            _gameStatistics = new List<GameStatistics>();

            Date = DateTime.Parse("1/1/1900");
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
    }
}