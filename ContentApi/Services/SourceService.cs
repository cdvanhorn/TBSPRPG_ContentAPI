using System;
using System.Threading.Tasks;
using ContentApi.Repositories;

namespace ContentApi.Services
{
    public interface ISourceService
    {
        public Task<string> GetSourceForKey(Guid key, string language = null); 
    }
    
    public class SourceService : ISourceService
    {
        private readonly ISourceRepository _repository;

        public SourceService(ISourceRepository repository)
        {
            _repository = repository;
        }
        
        public Task<string> GetSourceForKey(Guid key, string language = null)
        {
            return _repository.GetSourceForKey(key, language);
        }
    }
}