using System.Collections.Generic;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Services
{
    public class HoleService : IHoleService
    {
        private readonly IHoleRepository _holeRepository;

        public HoleService(IHoleRepository holeRepository)
        {
            _holeRepository = holeRepository;
        }

        public IEnumerable<Hole> GetHoles()
        {
            return _holeRepository.All();
        }
    }
}