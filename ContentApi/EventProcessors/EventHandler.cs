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
        protected IGameService _gameService;
        protected IGameAggregateAdapter _gameAggregateAdapter;
        
        //entities to be used by the handlers
        protected Game _game;
        protected Location _location;

        public EventHandler(IContentService contentService, ISourceService sourceService, IGameService gameService)
        {
            _contentService = contentService;
            _sourceService = sourceService;
            _gameService = gameService;
            _gameAggregateAdapter = new GameAggregateAdapter();
        }

        public Task HandleEvent(GameAggregate gameAggregate, Event evnt)
        {
            _game = _gameAggregateAdapter.ToEntity(gameAggregate);
            _location = _gameAggregateAdapter.ToLocation(gameAggregate);
            return HandleEvent(evnt);
        }

        protected abstract Task HandleEvent(Event evnt);
    }
}