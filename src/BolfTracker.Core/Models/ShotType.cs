using System.Collections.Generic;

namespace BolfTracker.Models
{
    public class ShotType : IEntity
    {
        private readonly ICollection<Shot> _shots;

        public ShotType()
        {
            _shots = new List<Shot>();
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
    }
}
