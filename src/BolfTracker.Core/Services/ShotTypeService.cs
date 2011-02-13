using System.Collections.Generic;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Services
{
    public class ShotTypeService : IShotTypeService
    {
        private readonly IShotTypeRepository _scoreTypeRepository;

        public ShotTypeService(IShotTypeRepository scoreTypeRepository)
        {
            _scoreTypeRepository = scoreTypeRepository;
        }

        public IEnumerable<ShotType> GetScoreTypes()
        {
            return _scoreTypeRepository.All();
        }
    }
}