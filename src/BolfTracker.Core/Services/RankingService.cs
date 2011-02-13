using System;
using System.Collections.Generic;
using System.Linq;

using BolfTracker.Infrastructure;
using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Services
{
    public class RankingService : IRankingService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IRankingRepository _rankingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RankingService(IGameRepository gameRepository, IRankingRepository rankingRepository, IUnitOfWork unitOfWork)
        {
            _gameRepository = gameRepository;
            _rankingRepository = rankingRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Ranking> GetRankings(int month, int year)
        {
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            return _rankingRepository.GetByMonthAndYear(month, year);
        }

        public void CalculateRankings(int month, int year)
        {
            // The easiest thing to do now is to just delete all of the rankings and insert the updated stats
            DeleteRankings(month, year);

            var games = _gameRepository.GetByMonthAndYear(month, year);
            var gamesStatistics = games.SelectMany(g => g.GameStatistics);
            var shots = games.SelectMany(g => g.Shots);
            var players = shots.Select(s => s.Player).Distinct();

            // The first thing that needs to be done is to find the eligibility line, which is half the average 
            // of games played by each player
            int eligibilityLine = 0;

            // The second thing we want to do is find the top ranked player because we will need their stats
            // to be able to calculate the "games back" for all other players
            decimal topPlayerWinningPercentage = 0.00M;
            int topPlayerWins = 0, topPlayerLosses = 0, topPlayerTotalPoints = 0, topPlayerPointsPerGame = 0;

            Action<decimal, int, int, int, int> setTopPlayerStats =
                (winningPercentage, wins, losses, totalPoints, pointsPerGame) =>
                {
                    topPlayerWinningPercentage = winningPercentage;
                    topPlayerWins = wins;
                    topPlayerLosses = losses;
                    topPlayerTotalPoints = totalPoints;
                    topPlayerPointsPerGame = pointsPerGame;
                };

            foreach (var player in players)
            {
                int wins = gamesStatistics.Count(gs => gs.Player.Id == player.Id && gs.Winner);
                int losses = gamesStatistics.Count(gs => gs.Player.Id == player.Id && !gs.Winner);
                int totalPoints = gamesStatistics.Where(gs => gs.Player.Id == player.Id).Sum(gs => gs.Points);
                int pointsPerGame = totalPoints / (wins + losses);

                decimal winningPercentage = (losses > 0) ? Decimal.Round(Convert.ToDecimal(wins) / Convert.ToDecimal(wins + losses), 3, MidpointRounding.AwayFromZero) : 1.00M;

                if (winningPercentage > topPlayerWinningPercentage)
                {
                    setTopPlayerStats(winningPercentage, wins, losses, totalPoints, pointsPerGame);
                }
                else if (winningPercentage == topPlayerWinningPercentage)
                {
                    if (pointsPerGame > topPlayerPointsPerGame)
                    {
                        setTopPlayerStats(winningPercentage, wins, losses, totalPoints, pointsPerGame);
                    }
                    else if (pointsPerGame == topPlayerPointsPerGame)
                    {
                        if (totalPoints > topPlayerTotalPoints)
                        {
                            setTopPlayerStats(winningPercentage, wins, losses, totalPoints, pointsPerGame);
                        }
                    }
                }
            }

            // Next go through each player and calculate ranking stats
            foreach (var player in players)
            {
                var ranking = new Ranking() { Player = player, Month = month, Year = year };

                ranking.Wins = gamesStatistics.Count(gs => gs.Player.Id == player.Id && gs.Winner);
                ranking.Losses = gamesStatistics.Count(gs => gs.Player.Id == player.Id && !gs.Winner);
                ranking.WinningPercentage = Decimal.Round(Convert.ToDecimal(ranking.Wins) / Convert.ToDecimal(ranking.TotalGames), 3, MidpointRounding.AwayFromZero);
                ranking.TotalPoints = gamesStatistics.Where(gs => gs.Player.Id == player.Id).Sum(gs => gs.Points);
                ranking.PointsPerGame = ranking.TotalPoints / ranking.TotalGames;

                var lastTenGames = gamesStatistics.Where(gs => gs.Player.Id == player.Id).OrderByDescending(gs => gs.Game.Date).Take(10);

                ranking.LastTenWins = lastTenGames.Count(gs => gs.Player.Id == player.Id && gs.Winner);
                ranking.LastTenLosses = lastTenGames.Count(gs => gs.Player.Id == player.Id && !gs.Winner);
                ranking.LastTenWinningPercentage = Decimal.Round(Convert.ToDecimal(ranking.LastTenWins) / Convert.ToDecimal(ranking.LastTenTotalGames), 3, MidpointRounding.AwayFromZero);

                if (ranking.WinningPercentage != topPlayerWinningPercentage)
                {
                    ranking.GamesBack = Decimal.Round((Convert.ToDecimal((topPlayerWins - topPlayerLosses) - (ranking.Wins - ranking.Losses))) / 2M, 1, MidpointRounding.AwayFromZero);
                }

                _rankingRepository.Add(ranking);
            }

            _unitOfWork.Commit();
        }

        private void DeleteRankings(int month, int year)
        {
            var rankings = _rankingRepository.GetByMonthAndYear(month, year);

            foreach (var ranking in rankings)
            {
                _rankingRepository.Delete(ranking);
            }

            _unitOfWork.Commit();
        }
    }
}