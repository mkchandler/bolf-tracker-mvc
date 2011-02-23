using System;
using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class PlayerViewModel
    {
        public PlayerViewModel(Player player)
        {
            Player = player;
        }

        public Player Player
        {
            get;
            private set;
        }

        public PlayerStatistics CurrentMonthPlayerStatistics
        {
            get
            {
                return Player.PlayerStatistics.SingleOrDefault(ps => ps.Player.Id == Player.Id && ps.Month == DateTime.Today.Month && ps.Year == DateTime.Today.Year);
            }
        }

        public IEnumerable<PlayerHoleStatistics> CurrentMonthPlayerHoleStatistics
        {
            get
            {
                return Player.PlayerHoleStatistics.Where(phs => phs.Player.Id == Player.Id && phs.Month == DateTime.Today.Month && phs.Year == DateTime.Today.Year);
            }
        }
    }
}