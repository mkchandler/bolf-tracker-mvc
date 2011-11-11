using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class GameRepository: RepositoryBase<Game>, IGameRepository
    {
        public GameRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }

        public IEnumerable<Game> GetByMonthAndYear(int month, int year)
        {
            //IQuery<IEnumerable<Game>> query = QueryFactory.CreateGamesByMonthAndYearQuery(month, year);
            var games = Database.Games.Where(game => game.Date.Month == month && game.Date.Year == year).ToList();

            return games;
        }

        public IEnumerable<Game> GetByMonthAndYearWithStatistics(int month, int year)
        {
            var games = Database.Games.Include(game => game.GameStatistics).Include(game => game.PlayerGameStatistics).Where(game => game.Date.Month == month && game.Date.Year == year).ToList();

            return games;
        }
    }
}