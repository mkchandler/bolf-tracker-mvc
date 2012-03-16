using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BolfTracker.Models
{
    public class Hole : IEntity
    {
        private readonly ICollection<Shot> _shots;
        private readonly ICollection<HoleStatistics> _holeStatistics;
        private readonly ICollection<PlayerHoleStatistics> _playerHoleStatistics;

        public Hole()
        {
            _shots = new List<Shot>();
            _holeStatistics = new List<HoleStatistics>();
            _playerHoleStatistics = new List<PlayerHoleStatistics>();
        }

        [Required]
        public int Id
        {
            get;
            set;
        }

        [Required]
        public int Par
        {
            get;
            set;
        }

        public ICollection<Shot> Shots
        {
            get { return _shots; }
        }

        public ICollection<HoleStatistics> HoleStatistics
        {
            get { return _holeStatistics; }
        }

        public ICollection<PlayerHoleStatistics> PlayerHoleStatistics
        {
            get { return _playerHoleStatistics; }
        }
    }
}
