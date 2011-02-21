using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Services
{
    public interface IHoleService
    {
        IEnumerable<Hole> GetHoles();

        IEnumerable<HoleStatistics> GetHoleStatistics(int month, int year);

        void CalculateHoleStatistics(int month, int year);
    }
}