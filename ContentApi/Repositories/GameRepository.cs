using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContentApi.Repositories
{
    public interface IGameRepository
    {
        Task<List<Game>> GetAllGames();
        Task<Game> GetGameById(Guid gameId);
        void AddGame(Game game);
        void SaveChanges();
    }
    
    public class GameRepository : IGameRepository
    {
        private readonly ContentContext _context;

        public GameRepository(ContentContext context)
        {
            _context = context;
        }

        public Task<List<Game>> GetAllGames()
        {
            return _context.Games.AsQueryable().ToListAsync();
        }

        public Task<Game> GetGameById(Guid gameId)
        {
            return _context.Games.AsQueryable().Where(g => g.Id == gameId).FirstOrDefaultAsync();
        }

        public void AddGame(Game game)
        {
            _context.Games.Add(game);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}