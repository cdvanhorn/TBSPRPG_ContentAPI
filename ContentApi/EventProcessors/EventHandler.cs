using System.Threading.Tasks;
using ContentApi.Adapters;
using ContentApi.Entities;
using ContentApi.Services;
using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;
using TbspRpgLib.Events.Content;

namespace ContentApi.EventProcessors {
    public interface IEventHandler {
        Task HandleEvent(GameAggregate gameAggregate, Event evnt);
    }

    public abstract class EventHandler
    {
        protected IContentService _contentService;
        protected ISourceService _sourceService;
        private IGameAggregateAdapter _gameAggregateAdapter;

        public EventHandler(IContentService contentService, ISourceService sourceService)
        {
            _contentService = contentService;
            _sourceService = sourceService;
            _gameAggregateAdapter = new GameAggregateAdapter();
        }

        public Task HandleEvent(GameAggregate gameAggregate, Event evnt)
        {
            return HandleEvent(
                _gameAggregateAdapter.ToEntity(gameAggregate),
                evnt);
        }

        protected abstract Task HandleEvent(Game game, Event evnt);
    }
}