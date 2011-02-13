using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Services
{
    public interface IHoleService
    {
        IEnumerable<Hole> GetHoles();
    }
}