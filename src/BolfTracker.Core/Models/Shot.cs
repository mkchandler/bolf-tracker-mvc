using System.ComponentModel.DataAnnotations;

namespace BolfTracker.Models
{
    public class Shot : IEntity
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual Game Game
        {
            get;
            set;
        }

        public virtual Player Player
        {
            get;
            set;
        }

        public virtual Hole Hole
        {
            get;
            set;
        }

        public virtual ShotType ShotType
        {
            get;
            set;
        }

        [Required]
        public virtual int Attempts
        {
            get;
            set;
        }

        public virtual bool ShotMade
        {
            get;
            set;
        }

        public virtual int Points
        {
            get;
            set;
        }                
    }
}