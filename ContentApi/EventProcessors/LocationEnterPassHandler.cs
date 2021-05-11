using TbspRpgLib.Events;
using TbspRpgLib.Aggregates;

using System.Threading.Tasks;

namespace ContentApi.EventProcessors {
    public interface ILocationEnterPassHandler : IEventHandler {

    }
    public class LocationEnterPassHandler : EventHandler, ILocationEnterPassHandler {
        public LocationEnterPassHandler(IAggregateService aggregateService) : base(aggregateService) {
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

//we will want different output depending on the game state
//How do we determine what content id to use
//there will be a bunch of code that looks like
//if game.foo == bar
//  content = baz;

//do we want to store the if statement in the content table then the content service will track
//game variables in a database table?

//or ask the map for the content and it will return some javascript to be executed

//we'll have a content table that is just a guid and text content, one table for each language
//we'll have a conditional table that will contain lua code that will produce the content id based on game state