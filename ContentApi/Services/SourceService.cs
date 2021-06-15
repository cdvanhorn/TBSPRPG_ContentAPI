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

        private const string INVALID_JAVASCRIPT = "invalid JavaScript {0}";
        private const string INVALID_JAVASCRIPT_RESULT = "invalid JavaScript result {0}: {1}";
        private const string INVALID_SOURCE_KEY = "invalid source key {0}";
        private const string ERROR_PREFIX = "invalid";
        private const string JAVASCRIPT_GAME_STATE_VARIABLE = "gameState";
        private const string JAVASCRIPT_FUNCTION_NAME = "eval";

        public SourceService(ISourceRepository repository, IConditionalSourceRepository conditionalSourceRepository)
        {
            _repository = repository;
            _csRepository = conditionalSourceRepository;
            _engine = new ScriptEngine();
            _gameStateJsAdapter = new GameStateJsAdapter();
        }

        private async Task<string> GetKeyViaJavaScript(Guid key, Game game)
        {
            var javaScript = await _csRepository.GetJavaScriptForKey(key);
            if (javaScript == null) return null;
            try
            {
                if (game != null)
                {
                    _engine.SetGlobalValue(
                        JAVASCRIPT_GAME_STATE_VARIABLE,
                        _gameStateJsAdapter.ToGameStateJs(game, _engine)
                    );
                }
                _engine.Evaluate(javaScript);
                return _engine.CallGlobalFunction<string>(JAVASCRIPT_FUNCTION_NAME);
            }
            catch (Exception e)
            {
                //TODO log the error
                return string.Format(INVALID_JAVASCRIPT, key);
            }
        }
        
        public async Task<string> GetSourceForKey(Guid key, Game game = null, string language = null)
        {
            var jsResult = await GetKeyViaJavaScript(key, game);
            if (jsResult != null && jsResult.StartsWith(ERROR_PREFIX))
                return jsResult;
            var backupKey = key;
            if (jsResult != null && !Guid.TryParse(jsResult, out key))
                return string.Format(INVALID_JAVASCRIPT_RESULT, backupKey, jsResult);

            var text = await _repository.GetSourceForKey(key, language);
            return text ?? string.Format(INVALID_SOURCE_KEY, key);
        }
    }
}