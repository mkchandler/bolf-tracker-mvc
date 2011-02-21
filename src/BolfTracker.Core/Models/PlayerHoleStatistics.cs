namespace BolfTracker.Models
{
    public class PlayerHoleStatistics : IEntity
    {
        public virtual int Id
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

        public virtual int Month
        {
            get;
            set;
        }

        public virtual int Year
        {
            get;
            set;
        }

        public virtual int ShotsMade
        {
            get;
            set;
        }

        public virtual int Attempts
        {
            get;
            set;
        }

        public virtual decimal ShootingPercentage
        {
            get;
            set;
        }

        public virtual int PointsScored
        {
            get;
            set;
        }

        public virtual int Pushes
        {
            get;
            set;
        }

        public virtual int Steals
        {
            get;
            set;
        }

        public virtual int SugarFreeSteals
        {
            get;
            set;
        }
    }
}