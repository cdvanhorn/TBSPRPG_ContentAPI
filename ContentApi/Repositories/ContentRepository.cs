using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentApi.Entities;
using Microsoft.EntityFrameworkCore;
using TbspRpgLib.Repositories;

namespace ContentApi.Repositories {
    public interface IContentRepository : IServiceTrackingRepository
    {
        Task<Content> GetLatestContentForGame(Guid gameId);
        Task<List<Content>> GetContentForGame(Guid gameId, int? offset = null, int? count = null);
        Task<List<Content>> GetContentForGameReverse(Guid gameId, int? offset = null, int? count = null);
        void AddContent(Content content);
        void SaveChanges();
    }

    public class ContentRepository : ServiceTrackingRepository, IContentRepository {
        private readonly ContentContext _context;

        public ContentRepository(ContentContext context) : base(context) {
            _context = context;
        }

        public Task<Content> GetLatestContentForGame(Guid gameId)
        {
            return _context.Contents.AsQueryable()
                .Where(c => c.GameId == gameId)
                .OrderByDescending(c => c.Position)
                .FirstOrDefaultAsync();
        }

        public Task<List<Content>> GetContentForGame(Guid gameId, int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<Content>> GetContentForGameReverse(Guid gameId, int? offset = null, int? count = null)
        {
            throw new NotImplementedException();
        }

        public void AddContent(Content content)
        {
            _context.Contents.Add(content);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
