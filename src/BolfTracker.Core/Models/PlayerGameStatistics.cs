namespace BolfTracker.Models
{
    public class PlayerGameStatistics : IEntity
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

        public int Points
        {
            get;
            set;
        }

        public bool Winner
        {
            get;
            set;
        }

        public bool OvertimeWin
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

        public int NormalSteals
        {
            get { return Steals - (SugarFreeSteals + StainlessSteals); }
        }

        public int SugarFreeSteals
        {
            get;
            set;
        }

        public int StainlessSteals
        {
            get;
            set;
        }

        public bool GameWinningSteal
        {
            get;
            set;
        }

        public bool Shutout
        {
            get;
            set;
        }

        public bool PerfectGame
        {
            get;
            set;
        }
    }
}
