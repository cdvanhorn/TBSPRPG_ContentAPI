using System.Threading.Tasks;
using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;
using TbspRpgLib.Events.Content;

namespace ContentApi.EventProcessors {
    public interface IEventHandler {
        Task HandleEvent(GameAggregate gameAggregate, Event evnt);
    }

    public class EventHandler {
        protected IAggregateService _aggregateService;

        public EventHandler(IAggregateService aggregateService) {
            _aggregateService = aggregateService;
        }

        protected async Task SendContentEvent(string eventId, string text, bool streamNew) {
            ContentContent eventContent = new ContentContent();
            eventContent.Id = eventId;
            eventContent.Text = text;
            var contentEvent = new ContentEvent(eventContent);
            await _aggregateService.AppendToAggregate(
                    AggregateService.CONTENT_AGGREGATE_TYPE,
                    contentEvent,
                    streamNew);

            // if(streamNew)
            //     await _eventService.SendEvent(contentEvent, true);
            // else
            //     await _eventService.SendEvent(contentEvent, false);
        }
    }
}