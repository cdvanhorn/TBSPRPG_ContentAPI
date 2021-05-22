using System;
using System.Threading.Tasks;
using ContentApi.Adapters;
using ContentApi.Entities;
using ContentApi.Repositories;
using Jurassic;

namespace ContentApi.Services
{
    public interface ISourceService
    {
        public Task<string> GetSourceForKey(Guid key, Game game = null, string language = null); 
    }
    
    public class SourceService : ISourceService
    {
        private readonly ISourceRepository _repository;
        private readonly IConditionalSourceRepository _csRepository;
        private readonly ScriptEngine _engine;
        private readonly IGameStateJsAdapter _gameStateJsAdapter;

        public SourceService(ISourceRepository repository, IConditionalSourceRepository conditionalSourceRepository)
        {
            _repository = repository;
            _csRepository = conditionalSourceRepository;
            _engine = new ScriptEngine();
            _gameStateJsAdapter = new GameStateJsAdapter();
        }
        
        public async Task<string> GetSourceForKey(Guid key, Game game = null, string language = null)
        {
            var javaScript = await _csRepository.GetJavaScriptForKey(key);
            if (javaScript != null)
            {
                try
                {
                    if (game != null)
                    {
                        _engine.SetGlobalValue(
                            "gameState",
                            _gameStateJsAdapter.ToGameStateJs(game, _engine)
                            );
                    }
                    _engine.Evaluate(javaScript);
                    var jsKey = _engine.CallGlobalFunction<string>("eval");
                    key = Guid.Parse(jsKey);
                }
                catch (Exception e)
                {
                    //TODO log the error
                    return $"invalid JavaScript {key}";
                }
            }
            
            var text = await _repository.GetSourceForKey(key, language);
            if(text == null)
                return $"invalid source key {key}";
            return text;
        }
    }
}