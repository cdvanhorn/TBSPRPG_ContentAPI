using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ContentApi.Repositories
{
    public interface IConditionalSourceRepository
    {
        Task<string> GetJavaScriptForKey(Guid contentKey);
    }
    
    public class ConditionalSourceRepository : IConditionalSourceRepository
    {
        private readonly ContentContext _context;

        public ConditionalSourceRepository(ContentContext context)
        {
            _context = context;
        }

        public Task<string> GetJavaScriptForKey(Guid contentKey)
        {
            return _context.ConditionalSources.AsQueryable()
                .Where(s => s.ContentKey == contentKey)
                .Select(s => s.JavaScript)
                .FirstOrDefaultAsync();
        }
    }
}