using ContentApi.Entities;
using Jurassic;

namespace ContentApi.Adapters
{
    public interface IGameStateJsAdapter
    {
        GameStateJs ToGameStateJs(Game game, ScriptEngine engine);
    }
    
    public class GameStateJsAdapter : IGameStateJsAdapter
    {
        public GameStateJs ToGameStateJs(Game game, ScriptEngine engine)
        {
            var gameStateJs = new GameStateJs(engine)
            {
                Id = game.Id.ToString(),
                AdventureId = game.AdventureId.ToString()
            };
            return gameStateJs;
        }
    }
}