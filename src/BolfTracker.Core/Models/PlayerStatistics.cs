namespace BolfTracker.Models
{
    public class PlayerStatistics : IEntity
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

        public int Points
        {
            get;
            set;
        }

        public decimal PointsPerGame
        {
            get;
            set;
        }

        public int Wins
        {
            get;
            set;
        }

        public int Losses
        {
            get;
            set;
        }

        public int TotalGames
        {
            get
            {
                return Wins + Losses;
            }
        }

        public decimal WinningPercentage
        {
            get;
            set;
        }

        public int Pushes
        {
            get;
            set;
        }

        public decimal PushesPerGame
        {
            get;
            set;
        }

        public int Steals
        {
            get;
            set;
        }

        public decimal StealsPerGame
        {
            get;
            set;
        }

        public int NormalSteals
        {
            get
            {
                return Steals - SugarFreeSteals;
            }
        }

        public decimal NormalStealsPerGame
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

        public int SugarFreeSteals
        {
            get;
            set;
        }

        public decimal SugarFreeStealsPerGame
        {
            get;
            set;
        }

        public int StainlessSteals
        {
            get;
            set;
        }

        public decimal StainlessStealsPerGame
        {
            get;
            set;
        }

        public int GameWinningSteals
        {
            get;
            set;
        }
    }
}
