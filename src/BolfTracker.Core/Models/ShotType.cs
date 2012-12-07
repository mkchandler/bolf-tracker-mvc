using System.Collections.Generic;

namespace BolfTracker.Models
{
    public class ShotType : IEntity
    {
        private readonly ICollection<Shot> _shots;
        private readonly ICollection<PlayerRivalryStatistics> _playerRivalryStatistics;

        public ShotType()
        {
            _shots = new List<Shot>();
            _playerRivalryStatistics = new List<PlayerRivalryStatistics>();
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

        public string Description
        {
            get;
            set;
        }

        public ICollection<Shot> Shots
        {
            get { return _shots; }
        }

        public ICollection<PlayerRivalryStatistics> PlayerRivalryStatistics
        {
            get { return _playerRivalryStatistics; }
        }
    }
}
