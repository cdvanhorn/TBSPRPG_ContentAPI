using System;
using System.Linq;
using System.Threading.Tasks;
using ContentApi.Entities;
using ContentApi.Entities.LanguageSources;
using Microsoft.EntityFrameworkCore;

namespace ContentApi.Repositories
{
    public interface ISourceRepository
    {
        public Task<string> GetSourceForKey(Guid key, string language = null);
    }
    
    public class SourceRepository : ISourceRepository
    {
        private readonly ContentContext _context;

        public SourceRepository(ContentContext context)
        {
            _context = context;
        }
        
        public Task<string> GetSourceForKey(Guid key, string language = null)
        {
            if (language == null || language == ENGLISH)
            {
                return _context.SourcesEn.AsQueryable()
                    .Where(s => s.ContentKey == key)
                    .Select(s => s.Text)
                    .FirstOrDefaultAsync();
            }
            
            if (language == SPANISH)
            {
                return _context.SourcesEsp.AsQueryable()
                    .Where(s => s.ContentKey == key)
                    .Select(s => s.Text)
                    .FirstOrDefaultAsync();
            }
            
            throw new ArgumentException($"invalid language {language}");
        }

        public static string ENGLISH = "en";
        public static string SPANISH = "esp";
    }
}