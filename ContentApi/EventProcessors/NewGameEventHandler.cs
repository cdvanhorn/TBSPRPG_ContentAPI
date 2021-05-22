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

        public NewGameEventHandler(IContentService contentService, IGameService gameService, ISourceService sourceService) :
            base(contentService, gameService, sourceService) {
        }

        protected override async Task HandleEvent(Game game, Event evnt) {
            //add the game to the database, if it doesn't already exist
            //add appropriate content to the database as well
            await _gameService.AddGame(game);

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