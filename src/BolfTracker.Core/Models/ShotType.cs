using System.Collections.Generic;

namespace BolfTracker.Models
{
    public class ShotType : IEntity
    {
        private ICollection<Shot> _shots;

        public ShotType()
        {
            _shots = new List<Shot>();
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

        public virtual string Description
        {
            get;
            set;
        }

        public virtual ICollection<Shot> Shots
        {
            get { return _shots; }
        }
    }
}