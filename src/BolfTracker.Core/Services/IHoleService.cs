using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Services
{
    public interface IHoleService
    {
        IEnumerable<Hole> GetHoles();

        IEnumerable<HoleStatistics> GetHoleStatistics();

        IEnumerable<HoleStatistics> GetHoleStatistics(int month, int year);

        Hole CreateHole(int holeNumber, int par);

        void CalculateHoleStatistics(int month, int year);
    }
}
