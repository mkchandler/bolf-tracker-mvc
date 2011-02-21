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
        public virtual int Id
        {
            get;
            set;
        }

        [Required]
        public virtual int Par
        {
            get;
            set;
        }

        public virtual ICollection<Shot> Shots
        {
            get { return _shots; }
        }

        public virtual ICollection<HoleStatistics> HoleStatistics
        {
            get { return _holeStatistics; }
        }

        public virtual ICollection<PlayerHoleStatistics> PlayerHoleStatistics
        {
            get { return _playerHoleStatistics; }
        }
    }
}