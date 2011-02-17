using System.Collections.Generic;

namespace BolfTracker.Models
{
    public class Player : IEntity
    {
        private readonly ICollection<Shot> _shots;
        private readonly ICollection<GameStatistics> _gameStatistics;
        private readonly ICollection<PlayerStatistics> _playerStatistics;
        private readonly ICollection<Ranking> _rankings;

        public Player()
        {
            _shots = new List<Shot>();
            _gameStatistics = new List<GameStatistics>();
            _playerStatistics = new List<PlayerStatistics>();
            _rankings = new List<Ranking>();
        }

        public virtual int Id
        {
            get;
            set;
        }

        public virtual string Name
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

        public virtual ICollection<PlayerStatistics> PlayerStatistics
        {
            get { return _playerStatistics; }
        }

        public virtual ICollection<Ranking> Rankings
        {
            get { return _rankings; }
        }
    }
}