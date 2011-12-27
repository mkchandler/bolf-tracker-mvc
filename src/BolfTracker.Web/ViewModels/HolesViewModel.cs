using System;
using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class HolesViewModel
    {
        public HolesViewModel(int month, int year, IEnumerable<HoleStatistics> monthHoleStatistics, IEnumerable<HoleStatistics> lifetimeHoleStatistics)
        {
            Month = month;
            Year = year;
            MonthHoleStatistics = monthHoleStatistics;
            LifetimeHoleStatistics = lifetimeHoleStatistics;
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

        public IEnumerable<HoleStatistics> MonthHoleStatistics
        {
            get;
            private set;
        }

        public IEnumerable<HoleStatistics> LifetimeHoleStatistics
        {
            get;
            private set;
        }
    }
}
