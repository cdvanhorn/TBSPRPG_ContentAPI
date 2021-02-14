using System.Threading.Tasks;
using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;

namespace ContentApi.EventProcessors {
    public interface IEventHandler {
        Task HandleEvent(GameAggregate gameAggregate, Event evnt);
    }

    public class EventHandler {
        

        public EventHandler() {
            
        }
    }
}