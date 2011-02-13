using System.Collections.Generic;

using BolfTracker.Infrastructure;
using BolfTracker.Models;
using BolfTracker.Repositories;
using System;

namespace BolfTracker.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlayerService(IPlayerRepository playerRepository, IUnitOfWork unitOfWork)
        {
            _playerRepository = playerRepository;
            _unitOfWork = unitOfWork;
        }

        public Player GetPlayer(int id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            return _playerRepository.GetById(id);
        }

        public IEnumerable<Player> GetPlayers()
        {
            return _playerRepository.All();
        }

        public Player Create(string name)
        {
            Check.Argument.IsNotNullOrEmpty(name, "name", "Player name must contain a value");

            var player = new Player() { Name = name };

            _playerRepository.Add(player);
            _unitOfWork.Commit();

            return player;
        }

        public Player Update(int id, string name)
        {
            var player = _playerRepository.GetById(id);

            player.Name = name;

            _unitOfWork.Commit();

            return player;
        }

        public void Delete(int id)
        {
            Check.Argument.IsNotZeroOrNegative(id, "id");

            _playerRepository.Delete(_playerRepository.GetById(id));

            _unitOfWork.Commit();
        }
    }
}