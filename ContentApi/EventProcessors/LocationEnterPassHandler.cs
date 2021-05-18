using System;
using TbspRpgLib.Events;
using TbspRpgLib.Aggregates;

using System.Threading.Tasks;
using ContentApi.Entities;
using ContentApi.Services;

namespace ContentApi.EventProcessors {
    public interface ILocationEnterPassHandler : IEventHandler {

    }
    public class LocationEnterPassHandler : EventHandler, ILocationEnterPassHandler {
        public LocationEnterPassHandler(IContentService contentService, IGameService gameService, ISourceService sourceService) :
            base(contentService, gameService, sourceService) {
        }

        protected override async Task HandleEvent(Game game, Event evnt)
        {
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
                Text = $"{game.Id} successfully entered a location"
            };
            await _contentService.AddContent(content);
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