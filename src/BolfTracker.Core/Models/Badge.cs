using System.Collections.Generic;

namespace BolfTracker.Models
{
    public class Badge : IEntity
    {
        private readonly ICollection<PlayerBadges> _playerBadges;

        public Badge()
        {
            _playerBadges = new List<PlayerBadges>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<PlayerBadges> PlayerBadges 
        {
            get { return _playerBadges; }
        }
    }
}
