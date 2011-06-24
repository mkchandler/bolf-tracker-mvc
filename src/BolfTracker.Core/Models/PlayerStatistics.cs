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

        public virtual decimal ShootingPercentage
        {
            get;
            set;
        }

        public virtual int Points
        {
            get;
            set;
        }

        public virtual decimal PointsPerGame
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

        public virtual decimal WinningPercentage
        {
            get;
            set;
        }

        public virtual int Pushes
        {
            get;
            set;
        }

        public virtual decimal PushesPerGame
        {
            get;
            set;
        }

        public virtual int Steals
        {
            get;
            set;
        }

        public virtual decimal StealsPerGame
        {
            get;
            set;
        }

        public virtual int NormalSteals
        {
            get
            {
                return Steals - SugarFreeSteals; 
            }
        }

        public virtual decimal NormalStealsPerGame
        {
            get
            {
                if (TotalGames > 0)
                {
                    return NormalSteals / TotalGames;
                }

                return 0;
            }
        }

        public virtual int SugarFreeSteals
        {
            get;
            set;
        }

        public virtual decimal SugarFreeStealsPerGame
        {
            get;
            set;
        }

        public virtual int StainlessSteals
        {
            get;
            set;
        }

        public virtual decimal StainlessStealsPerGame
        {
            get;
            set;
        }

        public virtual int GameWinningSteals
        {
            get;
            set;
        }
    }
}