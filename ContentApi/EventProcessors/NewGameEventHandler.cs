using System;
using System.Threading.Tasks;

using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;
using TbspRpgLib.Events.Content;

namespace ContentApi.EventProcessors {
    public interface INewGameEventHandler : IEventHandler {

    }

    public class NewGameEventHandler : EventHandler, INewGameEventHandler {
        protected IEventService _eventService;

        public NewGameEventHandler(IEventService eventService) : base() {
            _eventService = eventService;
        }

        public async Task HandleEvent(GameAggregate gameAggregate, Event evnt) {
            //need to create a new event stream that will be the content for this game
            //will eventually call the adventure api to get the opening credits
            //for now create a new content event and send it
            ContentContent eventContent = new ContentContent();
            eventContent.Id = gameAggregate.Id;
            eventContent.Text = $"New game started with id {gameAggregate.Id} of adventure {gameAggregate.AdventureId}";
            var contentEvent = new ContentEvent(eventContent);
            //this should be a new stream
            await _eventService.SendEvent(contentEvent, true);
        }
    }
}