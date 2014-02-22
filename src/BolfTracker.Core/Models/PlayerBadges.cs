namespace BolfTracker.Models
{
    public class PlayerBadges : IEntity
    {
        public int Id { get; set; }

        public Player Player { get; set; }

        public Badge Badge { get; set; }

        public Game Game { get; set; }
    }
}
