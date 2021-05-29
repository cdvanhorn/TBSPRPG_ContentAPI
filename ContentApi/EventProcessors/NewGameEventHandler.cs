using System;
using System.Threading.Tasks;
using ContentApi.Entities;
using ContentApi.Services;
using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;
using TbspRpgLib.Events.Content;

namespace ContentApi.EventProcessors {
    public interface INewGameEventHandler : IEventHandler {

    }

    public class NewGameEventHandler : EventHandler, INewGameEventHandler {

        public NewGameEventHandler(IContentService contentService, ISourceService sourceService) :
            base(contentService, sourceService) {
        }

        protected override async Task HandleEvent(Game game, Event evnt) {
            var content = new Content()
            {
                GameId = game.Id,
                Position = evnt.StreamPosition,
                Text = await _sourceService.GetSourceForKey(game.AdventureId)
            };
            await _contentService.AddContent(content);
        }
    }
}