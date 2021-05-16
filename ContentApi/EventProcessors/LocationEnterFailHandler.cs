using System;
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
            var dbGame = await _gameService.GetGameById(game.Id);
            if (dbGame == null)
            {
                throw new Exception("can't process event before game in database");
            }
            //add some content to the database
            var content = new Content()
            {
                GameId = game.Id,
                Position = evnt.StreamPosition,
                Text = $"{game.Id} unsuccessfully entered a location"
            };
            await _contentService.AddContent(content);
        }
    }
}