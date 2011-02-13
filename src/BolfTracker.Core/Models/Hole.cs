using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BolfTracker.Models
{
    public class Hole : IEntity
    {
        private ICollection<Shot> _shots;

        public Hole()
        {
            _shots = new List<Shot>();
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
    }
}