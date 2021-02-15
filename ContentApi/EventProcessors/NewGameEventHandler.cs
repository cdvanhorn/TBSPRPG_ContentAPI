using System;
using System.Threading.Tasks;

using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;
using TbspRpgLib.Events.Content;

namespace ContentApi.EventProcessors {
    public interface INewGameEventHandler : IEventHandler {

    }

    public class NewGameEventHandler : EventHandler, INewGameEventHandler {

        public NewGameEventHandler(IEventService eventService) : base(eventService) {
        }

        public async Task HandleEvent(GameAggregate gameAggregate, Event evnt) {
            //need to create a new event stream that will be the content for this game
            //will eventually call the adventure api to get the opening credits
            //for now create a new content event and send it
            var eventId = gameAggregate.Id;
            var eventText = $"New game started with id {gameAggregate.Id} of adventure {gameAggregate.AdventureId}";
            await SendContentEvent(eventId, eventText, true);
        }
    }
}