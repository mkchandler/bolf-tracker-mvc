using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Services
{
    public interface IShotTypeService
    {
        IEnumerable<ShotType> GetScoreTypes();
    }
}