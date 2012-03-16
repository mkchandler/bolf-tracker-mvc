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

        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string UrlFriendlyName
        {
            get
            {
                return Name.Trim().ToLower().Replace(' ', '-');
            }
        }
        
        public string Initials
        {
            get;
            set;
        }

        public ICollection<Shot> Shots
        {
            get { return _shots; }
        }

        public ICollection<PlayerStatistics> PlayerStatistics
        {
            get { return _playerStatistics; }
        }

        public ICollection<PlayerHoleStatistics> PlayerHoleStatistics
        {
            get { return _playerHoleStatistics; }
        }

        public ICollection<PlayerGameStatistics> PlayerGameStatistics
        {
            get { return _playerGameStatistics; }
        }

        public ICollection<PlayerCareerStatistics> PlayerCareerStatistics
        {
            get { return _playerCareerStatistics; }
        }

        public ICollection<Ranking> Rankings
        {
            get { return _rankings; }
        }
    }
}
