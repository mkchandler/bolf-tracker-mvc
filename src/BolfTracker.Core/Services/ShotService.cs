using System;
using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Services
{
    public class ShotService : IShotService
    {
        private readonly IShotRepository _shotRepository;
        private readonly IShotTypeRepository _shotTypeRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IHoleRepository _holeRepository;

        // TODO: Really need to figure out a better way to do this
        private const int ShotTypeMake = 1;
        private const int ShotTypeMiss = 2;
        private const int ShotTypePush = 3;
        private const int ShotTypeSteal = 4;
        private const int ShotTypeSugarFreeSteal = 5;

        public ShotService(IShotRepository shotRepository, IShotTypeRepository shotTypeRepository, IGameRepository gameRepository, IPlayerRepository playerRepository, IHoleRepository holeRepository)
        {
            _shotRepository = shotRepository;
            _shotTypeRepository = shotTypeRepository;
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
            _holeRepository = holeRepository;
        }

        public Shot GetShot(int id)
        {
            return _shotRepository.GetById(id);
        }

        public IEnumerable<Shot> GetShots(int gameId)
        {
            return _shotRepository.GetByGame(gameId);
        }

        public void RecalculateShots(int gameId)
        {
            throw new NotImplementedException();
        }

        public void Create(int gameId, int playerId, int holeId, int attempts, bool shotMade)
        {
            var game = _gameRepository.GetById(gameId);
            var shots = _shotRepository.GetByGame(gameId);
            var player = _playerRepository.GetById(playerId);
            var hole = _holeRepository.GetById(holeId);

            var allHoles = _holeRepository.All();
            var allShotTypes = _shotTypeRepository.All();

            int playerCurrentPoints = shots.Where(s => s.Player.Id == player.Id).Sum(s => s.Points);
            int totalPoints = allHoles.Where(h => h.Id <= hole.Id).Sum(h => h.Par);
            int totalPointsTaken = shots.Sum(s => s.Points);
            int pointsAvailable = totalPoints - totalPointsTaken;

            var currentShot = new Shot
            {
                Game = game,
                Player = player,
                Hole = hole,
                Attempts = attempts,
                ShotMade = shotMade
            };

            // If first shot of the game, logic is easy
            if (!shots.Any())
            {
                if (currentShot.ShotMade)
                {
                    currentShot.ShotType = allShotTypes.Single(st => st.Id == ShotTypeMake);
                    currentShot.Points = hole.Par;
                }
                else
                {
                    currentShot.ShotType = allShotTypes.Single(st => st.Id == ShotTypeMiss);
                }
            }
            else
            {
                // If not the first shot, stuff starts getting a little complicated

                // If the shot was made, start the logic, else just put in a missed shot
                if (currentShot.ShotMade)
                {
                    // First lets check if it is the first shot of the current hole
                    if (shots.Any(s => s.Hole.Id == currentShot.Hole.Id && s.ShotMade == true))
                    {
                        // If it's not the first shot, figure out if it is a push
                        var currentLowestShotMade = shots.Where(s => s.Hole.Id == currentShot.Hole.Id && s.ShotMade == true).OrderBy(s => s.Attempts).First();

                        if (currentLowestShotMade.Attempts == currentShot.Attempts)
                        {
                            // This is a push
                            currentShot.ShotType = allShotTypes.Single(st => st.Id == ShotTypePush);
                            currentShot.Points = pointsAvailable;

                            // Zero out other player's points that this player pushed.
                            Update(currentLowestShotMade.Id, 0, allShotTypes.Single(st => st.Id == ShotTypeMake));
                        }
                        else
                        {
                            // Then it is a steal (because someone has already made it if we got to this point)

                            // TODO: Need to account for StainlessSteals here

                            // First figure out if the hole had been pushed, because then it will be a "Sugar-Free Steal"
                            if (shots.Any(s => s.Hole.Id == currentShot.Hole.Id && s.ShotType.Id == ShotTypePush))
                            {
                                // This is a sugar-free steal
                                currentShot.ShotType = allShotTypes.Single(st => st.Id == ShotTypeSugarFreeSteal);
                                currentShot.Points = pointsAvailable;

                                // Reset the person who had a push to just be a "Make" now
                                var playerWithPush = shots.Single(s => s.Hole.Id == currentShot.Hole.Id && s.ShotType.Id == ShotTypePush);
                                Update(playerWithPush.Id, 0, allShotTypes.Single(st => st.Id == ShotTypeMake));
                            }
                            else
                            {
                                var playerWithPoints = shots.Single(s => s.Hole.Id == currentShot.Hole.Id && s.Points > 0);
                                
                                // This is a regular steal
                                currentShot.ShotType = allShotTypes.Single(st => st.Id == ShotTypeSteal);
                                currentShot.Points = playerWithPoints.Points;

                                // Reset the score to zero for the player who previously had the points
                                Update(playerWithPoints.Id, 0, allShotTypes.Single(st => st.Id == ShotTypeMake));
                            }
                        }
                    }
                    else
                    {
                        // If it is the first shot on the hole, set type and sum points
                        if (currentShot.ShotMade)
                        {
                            currentShot.ShotType = allShotTypes.Single(st => st.Id == ShotTypeMake);
                            currentShot.Points = pointsAvailable;
                        }
                        else
                        {
                            currentShot.ShotType = allShotTypes.Single(st => st.Id == ShotTypeMiss);
                        }
                    }
                }
                else
                {
                    currentShot.ShotType = allShotTypes.Single(st => st.Id == ShotTypeMiss);
                }
            }

            _shotRepository.Add(currentShot);
        }

        public void Update(int id, int points, ShotType shotType)
        {
            var shot = _shotRepository.GetById(id);

            shot.Points = points;
            shot.ShotType = shotType;

            _shotRepository.Update(shot);
        }

        public void Delete(int id)
        {
            _shotRepository.Delete(id);
        }


        public void DeleteToShot(int gameId, int shotId)
        {
            var shots = _shotRepository.GetByGame(gameId);
            var shotToDelete = _shotRepository.GetById(shotId);

            //Get shots to reapply after deleting, do this if the shot we are deleting may be a steal from a previous push
            //that is no longer identified as a push.
            var shotsToReapply = shots.Where(s => s.Hole.Id == shotToDelete.Hole.Id && s.Id < shotToDelete.Id).ToList();
            
            //Now find first shot on this hole and delete to there
            var firstShotOnHole = shots.Where(s => s.Hole.Id == shotToDelete.Hole.Id).Min(s => s.Id);
            
            //Delete all shots up this the first shot on the hole where the shot to be deleted was shot (yep, makes no sense)
            _shotRepository.DeleteToShot(gameId,firstShotOnHole);

            //Reapply the shots on the hole that we really don't want to delete so the shot type and score will be correct.
            foreach(var shot in shotsToReapply.OrderBy(s => s.Id))
            {
                Create(shot.Game.Id, shot.Player.Id, shot.Hole.Id, shot.Attempts, shot.ShotMade);
            }
        }
    }
}
