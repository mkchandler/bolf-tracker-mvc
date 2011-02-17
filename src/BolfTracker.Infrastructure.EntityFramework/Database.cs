using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.EntityClient;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class Database : ObjectContext
    {
        private IObjectSet<Game> _games;
        private IObjectSet<Hole> _holes;
        private IObjectSet<Player> _players;
        private IObjectSet<Shot> _shots;
        private IObjectSet<Ranking> _rankings;
        private IObjectSet<GameStatistics> _gameStatistics;
        private IObjectSet<PlayerStatistics> _playerStatistics;

        public Database(EntityConnection connection) : base(connection)
        {
            ContextOptions.LazyLoadingEnabled = true;
            ContextOptions.ProxyCreationEnabled = true;
        }

        public IObjectSet<Game> Games
        {
            get
            {
                return _games ?? (_games = ObjectSet<Game>());
            }
        }

        public IObjectSet<Hole> Holes
        {
            get
            {
                return _holes ?? (_holes = ObjectSet<Hole>());
            }
        }

        public IObjectSet<Player> Players
        {
            get
            {
                return _players ?? (_players = ObjectSet<Player>());
            }
        }

        public IObjectSet<Shot> Shots
        {
            get
            {
                return _shots ?? (_shots = ObjectSet<Shot>());
            }
        }

        public IObjectSet<Ranking> Rankings
        {
            get
            {
                return _rankings ?? (_rankings = ObjectSet<Ranking>());
            }
        }

        public IObjectSet<GameStatistics> GameStatistics
        {
            get
            {
                return _gameStatistics ?? (_gameStatistics = ObjectSet<GameStatistics>());
            }
        }

        public IObjectSet<PlayerStatistics> PlayerStatistics
        {
            get
            {
                return _playerStatistics ?? (_playerStatistics = ObjectSet<PlayerStatistics>());
            }
        }

        public virtual IObjectSet<TModel> ObjectSet<TModel>() where TModel : class
        {
            return CreateObjectSet<TModel>();
        }

        public virtual void Commit()
        {
            SaveChanges();
        }
    }
}