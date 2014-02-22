using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BolfTracker.Models
{
    public class PlayerBadges : IEntity
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int BadgeId { get; set; }
        public int? GameId { get; set; }
        public virtual Player Player { get; set; }
        public virtual Badge Badge { get; set; }
        public virtual Game Game { get; set; }

    }
}
