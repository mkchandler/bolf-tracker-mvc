using System;
using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class HolesViewModel
    {
        public HolesViewModel(int month, int year, IEnumerable<Hole> holes, IEnumerable<HoleStatistics> holeStatistics)
        {
            Month = month;
            Year = year;
            Holes = holes;
            CurrentMonthHoleStatistics = holeStatistics;
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

        public IEnumerable<Hole> Holes
        {
            get;
            private set;
        }

        public IEnumerable<HoleStatistics> CurrentMonthHoleStatistics
        {
            get;
            private set;
        }
    }
}