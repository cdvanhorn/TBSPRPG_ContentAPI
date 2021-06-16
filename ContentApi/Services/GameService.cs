using System;
using System.Threading.Tasks;
using ContentApi.Entities;
using ContentApi.Repositories;

namespace ContentApi.Services
{
    public interface IGameService
    {
        Task<Game> GetGameById(Guid gameId);
        Task AddGame(Game game);
    }
    
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public Task<Game> GetGameById(Guid gameId)
        {
            return _gameRepository.GetGameById(gameId);
        }

        public async Task AddGame(Game game)
        {
            var dbGame = await GetGameById(game.Id);
            if (dbGame == null)
            {
                _gameRepository.AddGame(game);
            }
        }
    }
}