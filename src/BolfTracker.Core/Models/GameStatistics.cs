namespace BolfTracker.Models
{
    public class GameStatistics : IEntity
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

        public virtual int HoleCount
        {
            get;
            set;
        }

        public virtual int OvertimeCount
        {
            get;
            set;
        }

        public virtual int PlayerCount
        {
            get;
            set;
        }

        public virtual int Points
        {
            get;
            set;
        }

        public virtual int ShotsMade
        {
            get;
            set;
        }

        public virtual int ShotsMissed
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

        public virtual int StainlessSteals
        {
            get;
            set;
        }
    }
}