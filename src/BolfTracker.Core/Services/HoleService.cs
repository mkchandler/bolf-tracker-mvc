using System;
using System.Collections.Generic;
using System.Linq;

using BolfTracker.Infrastructure;
using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Services
{
    public class HoleService : IHoleService
    {
        private readonly IHoleRepository _holeRepository;
        private readonly IHoleStatisticsRepository _holeStatisticsRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IShotRepository _shotRepository;

        public HoleService(IHoleRepository holeRepository, IHoleStatisticsRepository holeStatisticsRepository, IGameRepository gameRepository, IShotRepository shotRepository)
        {
            _holeRepository = holeRepository;
            _holeStatisticsRepository = holeStatisticsRepository;
            _gameRepository = gameRepository;
            _shotRepository = shotRepository;
        }

        public IEnumerable<Hole> GetHoles()
        {
            return _holeRepository.All().ToList();
        }

        public IEnumerable<HoleStatistics> GetHoleStatistics()
        {
            return _holeStatisticsRepository.All();
        }

        public IEnumerable<HoleStatistics> GetHoleStatistics(int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            return _holeStatisticsRepository.GetByMonthAndYear(month, year);
        }

        public Hole CreateHole(int holeNumber, int par)
        {
            Check.Argument.IsNotZeroOrNegative(holeNumber, "holeNumber");
            Check.Argument.IsNotZeroOrNegative(par, "par");

            var hole = new Hole() { Id = holeNumber, Par = par };

            _holeRepository.Add(hole);

            return hole;
        }

        public void CalculateHoleStatistics()
        {
            var months = _gameRepository.GetActiveMonthsAndYears();

            foreach (var month in months)
            {
                CalculateHoleStatistics(month.Item1, month.Item2);
            }
        }

        public void CalculateHoleStatistics(int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            DeleteHoleStatistics(month, year);

            var holes = _holeRepository.All().ToList();
            var shots = _shotRepository.GetByMonthAndYear(month, year);

            foreach (var hole in holes)
            {
                var holeShots = shots.Where(s => s.Hole.Id == hole.Id);

                if (holeShots.Any())
                {
                    var holeStatistics = new HoleStatistics() { Hole = hole, Month = month, Year = year };

                    holeStatistics.ShotsMade = holeShots.Count(s => s.ShotMade);
                    holeStatistics.Attempts = holeShots.Sum(s => s.Attempts);
                    holeStatistics.ShootingPercentage = Decimal.Round((decimal)holeStatistics.ShotsMade / (decimal)holeStatistics.Attempts, 3, MidpointRounding.AwayFromZero);
                    holeStatistics.PointsScored = holeShots.Sum(s => s.Points);
                    holeStatistics.Pushes = holeShots.Count(s => s.ShotType.Id == 3);
                    holeStatistics.Steals = holeShots.Count(s => s.ShotType.Id == 4);
                    holeStatistics.SugarFreeSteals = holeShots.Count(s => s.ShotType.Id == 5);

                    _holeStatisticsRepository.Add(holeStatistics);
                }
            }
        }

        private void DeleteHoleStatistics(int month, int year)
        {
            _holeStatisticsRepository.DeleteByMonthAndYear(month, year);
        }
    }
}
