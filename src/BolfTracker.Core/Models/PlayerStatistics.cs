namespace BolfTracker.Models
{
    public class PlayerStatistics : IEntity
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

        public virtual int Points
        {
            get;
            set;
        }

        public virtual int Wins
        {
            get;
            set;
        }

        public virtual int Losses
        {
            get;
            set;
        }

        public virtual int TotalGames
        {
            get
            {
                return Wins + Losses;
            }
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