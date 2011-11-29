using System;
using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class PlayerViewModel
    {
        public PlayerViewModel(int month, int year, Player player, PlayerStatistics playerStatistics, PlayerCareerStatistics playerCareerStatistics, IEnumerable<PlayerHoleStatistics> playerHoleStatistics)
        {
            Month = month;
            Year = year;
            Player = player;
            PlayerStatistics = playerStatistics;
            PlayerCareerStatistics = playerCareerStatistics;
            PlayerHoleStatistics = playerHoleStatistics;
        }

        public int Month
        {
            get;
            private set;
        }

        public string MonthName
        {
            get
            {
                return new DateTime(1, Month, 1).ToString("MMMM");
            }
        }

        public int Year
        {
            get;
            private set;
        }

        public Player Player
        {
            get;
            private set;
        }

        public PlayerStatistics PlayerStatistics
        {
            get;
            private set;
        }

        public PlayerCareerStatistics PlayerCareerStatistics
        {
            get;
            private set;
        }

        public IEnumerable<PlayerHoleStatistics> PlayerHoleStatistics
        {
            get;
            private set;
        }
    }
}