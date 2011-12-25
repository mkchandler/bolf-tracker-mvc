﻿using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Repositories
{
    public interface IGameRepository : IRepository<Game>
    {
        IEnumerable<Game> GetAllFinalized();

        IEnumerable<Game> GetByMonthAndYear(int month, int year);

        IEnumerable<Game> GetFinalizedByMonthAndYear(int month, int year);
    }
}
