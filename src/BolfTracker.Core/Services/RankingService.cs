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
        private readonly IShotRepository _shotRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IRankingRepository _rankingRepository;
        private readonly IPlayerGameStatisticsRepository _playerGameStatisticsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RankingService(IGameRepository gameRepository, IShotRepository shotRepository, IPlayerRepository playerRepository, IRankingRepository rankingRepository, IPlayerGameStatisticsRepository playerGameStatisticsRepository, IUnitOfWork unitOfWork)
        {
            _gameRepository = gameRepository;
            _shotRepository = shotRepository;
            _playerRepository = playerRepository;
            _rankingRepository = rankingRepository;
            _playerGameStatisticsRepository = playerGameStatisticsRepository;
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

            var games = _gameRepository.GetFinalizedByMonthAndYear(month, year);
            var playerGameStatistics = _playerGameStatisticsRepository.GetByMonthAndYear(month, year);
            var shots = _shotRepository.GetByMonthAndYear(month, year);
            var players = _playerRepository.GetActiveByMonthAndYear(month, year);

            // The eligibility line will only sample the top half of players sorted by number of games played
            int eligibilityLine = DetermineEligibilityLine(games, players, playerGameStatistics);
            
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
                int wins = playerGameStatistics.Count(gs => gs.Player.Id == player.Id && gs.Winner);
                int losses = playerGameStatistics.Count(gs => gs.Player.Id == player.Id && !gs.Winner);
                int totalPoints = playerGameStatistics.Where(gs => gs.Player.Id == player.Id).Sum(gs => gs.Points);
                int pointsPerGame = totalPoints / (wins + losses);

                decimal winningPercentage = (losses > 0) ? Decimal.Round(Convert.ToDecimal(wins) / Convert.ToDecimal(wins + losses), 3, MidpointRounding.AwayFromZero) : 1.00M;

                // Player must first be eligible to be determined as the top player
                if ((wins + losses) >= eligibilityLine)
                {
                    // From here on down, use our top player algorithm to determine result (winning percentage --> ppg --> total points)
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
            }

            // Next go through each player and calculate ranking stats to be put in the database
            foreach (var player in players)
            {
                var ranking = new Ranking() { Player = player, Month = month, Year = year };

                ranking.Wins = playerGameStatistics.Count(gs => gs.Player.Id == player.Id && gs.Winner);
                ranking.Losses = playerGameStatistics.Count(gs => gs.Player.Id == player.Id && !gs.Winner);
                ranking.WinningPercentage = Decimal.Round((decimal)ranking.Wins / (decimal)ranking.TotalGames, 3, MidpointRounding.AwayFromZero);
                ranking.TotalPoints = playerGameStatistics.Where(gs => gs.Player.Id == player.Id).Sum(gs => gs.Points);
                ranking.PointsPerGame = ranking.TotalPoints / ranking.TotalGames;
                ranking.Eligible = ranking.TotalGames >= eligibilityLine;

                var lastTenGames = playerGameStatistics.Where(gs => gs.Player.Id == player.Id).OrderByDescending(gs => gs.Game.Date).Take(10);

                ranking.LastTenWins = lastTenGames.Count(gs => gs.Player.Id == player.Id && gs.Winner);
                ranking.LastTenLosses = lastTenGames.Count(gs => gs.Player.Id == player.Id && !gs.Winner);
                ranking.LastTenWinningPercentage = Decimal.Round((decimal)ranking.LastTenWins / (decimal)ranking.LastTenTotalGames, 3, MidpointRounding.AwayFromZero);

                if (ranking.WinningPercentage != topPlayerWinningPercentage)
                {
                    ranking.GamesBack = Decimal.Round((Convert.ToDecimal((topPlayerWins - topPlayerLosses) - (ranking.Wins - ranking.Losses))) / 2M, 1, MidpointRounding.AwayFromZero);
                }

                _rankingRepository.Add(ranking);
            }

            _unitOfWork.Commit();
        }

        private int DetermineEligibilityLine(IEnumerable<Game> games, IEnumerable<Player> players, IEnumerable<PlayerGameStatistics> playerGameStatistics)
        {
            var gameCounts = new List<int>();

            foreach (var player in players)
            {
                //int playerGameCount = games.Count(g => g.Shots.Count(s => s.Player.Id == player.Id) > 0);
                int playerGameCount = playerGameStatistics.Count(pgs => pgs.Player.Id == player.Id);

                gameCounts.Add(playerGameCount);
            }

            // This will put the game counts in a descending order
            gameCounts.Sort();
            gameCounts.Reverse();

            // We only want to take the top half of the games played, but we will always round up
            var gameCountsToSample = gameCounts.Take(Convert.ToInt32(Decimal.Ceiling(Decimal.Round(Convert.ToDecimal(gameCounts.Count) / 2M, 1, MidpointRounding.AwayFromZero))));

            // The line is half the average of the game counts determined by the above formula
            int eligibilityLine = Convert.ToInt32(Decimal.Round((Decimal.Round(gameCountsToSample.Sum() / gameCountsToSample.Count(), 1, MidpointRounding.AwayFromZero)) * .5M, 0, MidpointRounding.AwayFromZero));

            return eligibilityLine;
        }

        private void DeleteRankings(int month, int year)
        {
            _rankingRepository.DeleteByMonthAndYear(month, year);
        }
    }
}