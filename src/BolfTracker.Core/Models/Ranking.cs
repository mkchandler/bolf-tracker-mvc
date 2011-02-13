namespace BolfTracker.Models
{
    public class Ranking : IEntity
    {
        public virtual int Id
        {
            get;
            set;
        }

        public virtual int Year
        {
            get;
            set;
        }

        public virtual int Month
        {
            get;
            set;
        }

        public virtual Player Player
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

        public virtual decimal WinningPercentage
        {
            get;
            set;
        }

        public virtual int TotalPoints
        {
            get;
            set;
        }

        public virtual int PointsPerGame
        {
            get;
            set;
        }

        public virtual decimal GamesBack
        {
            get;
            set;
        }

        public virtual bool Eligible
        {
            get;
            set;
        }

        public virtual int LastTenWins
        {
            get;
            set;
        }

        public virtual int LastTenLosses
        {
            get;
            set;
        }

        public virtual decimal LastTenWinningPercentage
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

        public virtual int LastTenTotalGames
        {
            get
            {
                return LastTenWins + LastTenLosses;
            }
        }
    }
}