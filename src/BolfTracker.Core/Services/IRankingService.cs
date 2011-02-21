using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Services
{
    public interface IRankingService
    {
        IEnumerable<Ranking> GetRankings(int month, int year);

        void CalculateRankings(int month, int year);
    }
}