namespace BolfTracker.Models
{
    public class PlayerRivalryStatistics : IEntity
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

        public Player AffectedPlayer
        {
            get;
            set;
        }

        public ShotType ShotType
        {
            get;
            set;
        }

        public Hole Hole
        {
            get;
            set;
        }

        public int Attempts
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
