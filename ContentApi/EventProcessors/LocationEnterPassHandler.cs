using TbspRpgLib.Events;
using TbspRpgLib.Aggregates;

using System.Threading.Tasks;

namespace ContentApi.EventProcessors {
    public interface ILocationEnterPassHandler : IEventHandler {

    }
    public class LocationEnterPassHandler : EventHandler, ILocationEnterPassHandler {
        public LocationEnterPassHandler(IEventService eventService) : base(eventService) {
        }

        public async Task HandleEvent(GameAggregate gameAggregate, Event evnt) {
            //need to create a new event stream that will be the content for this game
            //will eventually call the adventure api to get the opening credits
            //for now create a new content event and send it
            var eventId = gameAggregate.Id;
            var eventText = $"{gameAggregate.Id} of adventure {gameAggregate.AdventureId} entered location {gameAggregate.CurrentLocation}";
            await SendContentEvent(eventId, eventText, false);
        }
    }
}