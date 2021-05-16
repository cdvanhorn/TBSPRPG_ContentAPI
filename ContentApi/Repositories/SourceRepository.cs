using System;
using System.Threading.Tasks;

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
            throw new NotImplementedException();
        }

        public readonly string ENGLISH = "en";
        public readonly string SPANISH = "esp";
    }
}