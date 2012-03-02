using System.ComponentModel.DataAnnotations;

namespace BolfTracker.Models
{
    public class Shot : IEntity
    {
        public int Id
        {
            get;
            set;
        }

        public Game Game
        {
            get;
            set;
        }

        public Player Player
        {
            get;
            set;
        }

        public Hole Hole
        {
            get;
            set;
        }

        public ShotType ShotType
        {
            get;
            set;
        }

        [Required]
        public int Attempts
        {
            get;
            set;
        }

        public bool ShotMade
        {
            get;
            set;
        }

        public int Points
        {
            get;
            set;
        }                
    }
}
