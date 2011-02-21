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
        private readonly IUnitOfWork _unitOfWork;

        public HoleService(IHoleRepository holeRepository, IHoleStatisticsRepository holeStatisticsRepository, IUnitOfWork unitOfWork)
        {
            _holeRepository = holeRepository;
            _holeStatisticsRepository = holeStatisticsRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Hole> GetHoles()
        {
            return _holeRepository.All();
        }

        public IEnumerable<HoleStatistics> GetHoleStatistics(int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            return _holeStatisticsRepository.GetByMonthAndYear(month, year);
        }

        public void CalculateHoleStatistics(int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            DeleteHoleStatistics(month, year);

            var holes = _holeRepository.All();

            foreach (var hole in holes)
            {
                var holeShots = hole.Shots.Where(s => s.Game.Date.Month == month && s.Game.Date.Year == year && s.Hole.Id == hole.Id);

                var holeStatistics = new HoleStatistics() { Hole = hole, Month = month, Year = year };

                if (holeShots.Any())
                {
                    holeStatistics.ShotsMade = holeShots.Count(s => s.ShotMade);
                    holeStatistics.Attempts = holeShots.Sum(s => s.Attempts);
                    holeStatistics.ShootingPercentage = Decimal.Round(Convert.ToDecimal(holeStatistics.ShotsMade) / Convert.ToDecimal(holeStatistics.Attempts), 3, MidpointRounding.AwayFromZero);
                    holeStatistics.PointsScored = holeShots.Sum(s => s.Points);
                    holeStatistics.Pushes = holeShots.Count(s => s.ShotType.Id == 3);
                    holeStatistics.Steals = holeShots.Count(s => s.ShotType.Id == 4);
                    holeStatistics.SugarFreeSteals = holeShots.Count(s => s.ShotType.Id == 5);
                }

                _holeStatisticsRepository.Add(holeStatistics);
            }

            _unitOfWork.Commit();
        }

        private void DeleteHoleStatistics(int month, int year)
        {
            var holeStatistics = _holeStatisticsRepository.GetByMonthAndYear(month, year);

            foreach (var holeStatistic in holeStatistics)
            {
                _holeStatisticsRepository.Delete(holeStatistic);
            }

            _unitOfWork.Commit();
        }
    }
}