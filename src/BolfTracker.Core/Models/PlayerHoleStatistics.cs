namespace BolfTracker.Models
{
    public class PlayerHoleStatistics : IEntity
    {
        public int Id
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

        public int Month
        {
            get;
            set;
        }

        public int Year
        {
            get;
            set;
        }

        public int ShotsMade
        {
            get;
            set;
        }

        public int Attempts
        {
            get;
            set;
        }

        public decimal ShootingPercentage
        {
            get;
            set;
        }

        public int PointsScored
        {
            get;
            set;
        }

        public int Pushes
        {
            get;
            set;
        }

        public int Steals
        {
            get;
            set;
        }

        public int SugarFreeSteals
        {
            get;
            set;
        }
    }
}
