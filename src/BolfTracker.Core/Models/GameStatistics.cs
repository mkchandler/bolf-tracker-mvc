namespace BolfTracker.Models
{
    public class GameStatistics : IEntity
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

        public int HoleCount
        {
            get;
            set;
        }

        public int OvertimeCount
        {
            get;
            set;
        }

        public int PlayerCount
        {
            get;
            set;
        }

        public int Points
        {
            get;
            set;
        }

        public int ShotsMade
        {
            get;
            set;
        }

        public int ShotsMissed
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
    }
}
