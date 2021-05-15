using TbspRpgLib.Events;
using TbspRpgLib.Aggregates;

using System.Threading.Tasks;
using ContentApi.Entities;
using ContentApi.Services;

namespace ContentApi.EventProcessors {
    public interface ILocationEnterFailHandler : IEventHandler {

    }
    public class LocationEnterFailHandler : EventHandler, ILocationEnterFailHandler {
        public LocationEnterFailHandler(IContentService contentService, IGameService gameService) :
            base(contentService, gameService) {
        }

        protected override async Task HandleEvent(Game game, Event evnt) {
            //need to create a new event stream that will be the content for this game
            //will eventually call the adventure api to get the opening credits
            //for now create a new content event and send it
            // var eventId = gameAggregate.Id;
            // var eventText = $"{gameAggregate.Id} of adventure {gameAggregate.AdventureId} failed to enter location";
            //await SendContentEvent(eventId, eventText, false);
        }
    }
}