using Jurassic;
using Jurassic.Library;

namespace ContentApi.Entities
{
    public class GameStateJs : ObjectInstance
    {
        public GameStateJs(ScriptEngine engine) : base(engine)
        {
            
        }
        
        public string Id { get; set; }
        public string AdventureId { get; set; }
    }
}