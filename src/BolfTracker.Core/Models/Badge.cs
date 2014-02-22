using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ICollection<PlayerBadges> PlayerBadges 
        {
            get { return _playerBadges; }
        }
    }
}
