using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Web
{
    public class HolesViewModel
    {
        public HolesViewModel(IEnumerable<Hole> holes, IEnumerable<HoleStatistics> holeStatistics)
        {
            Holes = holes;
            CurrentMonthHoleStatistics = holeStatistics;
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