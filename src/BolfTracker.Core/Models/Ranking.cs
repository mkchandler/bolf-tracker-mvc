namespace BolfTracker.Models
{
    public class Ranking : IEntity
    {
        public int Id
        {
            get;
            set;
        }

        public int Year
        {
            get;
            set;
        }

        public int Month
        {
            get;
            set;
        }

        public Player Player
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

        public decimal WinningPercentage
        {
            get;
            set;
        }

        public int TotalPoints
        {
            get;
            set;
        }

        public int PointsPerGame
        {
            get;
            set;
        }

        public decimal GamesBack
        {
            get;
            set;
        }

        public bool Eligible
        {
            get;
            set;
        }

        public int LastTenWins
        {
            get;
            set;
        }

        public int LastTenLosses
        {
            get;
            set;
        }

        public decimal LastTenWinningPercentage
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

        public int LastTenTotalGames
        {
            get
            {
                return LastTenWins + LastTenLosses;
            }
        }
    }
}
