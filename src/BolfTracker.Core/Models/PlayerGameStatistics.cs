namespace BolfTracker.Models
{
    public class PlayerGameStatistics : IEntity
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

        public virtual Player Player
        {
            get;
            set;
        }

        public virtual int Points
        {
            get;
            set;
        }

        public virtual bool Winner
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

        public virtual bool GameWinningSteal
        {
            get;
            set;
        }
    }
}