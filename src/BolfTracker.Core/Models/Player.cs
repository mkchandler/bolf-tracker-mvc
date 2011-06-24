using System.Collections.Generic;

namespace BolfTracker.Models
{
    public class Player : IEntity
    {
        private readonly ICollection<Shot> _shots;
        private readonly ICollection<PlayerStatistics> _playerStatistics;
        private readonly ICollection<PlayerHoleStatistics> _playerHoleStatistics;
        private readonly ICollection<PlayerGameStatistics> _playerGameStatistics;
        private readonly ICollection<PlayerCareerStatistics> _playerCareerStatistics;
        private readonly ICollection<Ranking> _rankings;

        public Player()
        {
            _shots = new List<Shot>();
            _playerStatistics = new List<PlayerStatistics>();
            _playerHoleStatistics = new List<PlayerHoleStatistics>();
            _playerGameStatistics = new List<PlayerGameStatistics>();
            _playerCareerStatistics = new List<PlayerCareerStatistics>();
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
        
        public virtual string Initials
        {
            get;
            set;
        }

        public virtual ICollection<Shot> Shots
        {
            get { return _shots; }
        }

        public virtual ICollection<PlayerStatistics> PlayerStatistics
        {
            get { return _playerStatistics; }
        }

        public virtual ICollection<PlayerHoleStatistics> PlayerHoleStatistics
        {
            get { return _playerHoleStatistics; }
        }

        public virtual ICollection<PlayerGameStatistics> PlayerGameStatistics
        {
            get { return _playerGameStatistics; }
        }

        public virtual ICollection<PlayerCareerStatistics> PlayerCareerStatistics
        {
            get { return _playerCareerStatistics; }
        }

        public virtual ICollection<Ranking> Rankings
        {
            get { return _rankings; }
        }
    }
}