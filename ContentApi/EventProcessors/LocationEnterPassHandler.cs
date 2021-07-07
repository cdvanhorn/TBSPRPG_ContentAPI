// using System;
// using TbspRpgLib.Events;
// using TbspRpgLib.Aggregates;
//
// using System.Threading.Tasks;
// using ContentApi.Entities;
// using ContentApi.Services;
//
// namespace ContentApi.EventProcessors {
//     public interface ILocationEnterPassHandler : IEventHandler {
//
//     }
//     public class LocationEnterPassHandler : EventHandler, ILocationEnterPassHandler {
//         public LocationEnterPassHandler(IContentService contentService, ISourceService sourceService, IGameService gameService) :
//             base(contentService, sourceService, gameService) {
//         }
//
//         protected override async Task HandleEvent(Event evnt)
//         {
//             //add some content to the database
//             var content = new Content()
//             {
//                 GameId = _game.Id,
//                 Position = evnt.StreamPosition,
//                 Text = await _sourceService.GetSourceForKey(_location.CurrentLocation)
//             };
//             await _contentService.AddContent(content);
//         }
//     }
// }
//
// //we will want different output depending on the game state
// //How do we determine what content id to use
// //there will be a bunch of code that looks like
// //if game.foo == bar
// //  content = baz;
//
// //do we want to store the if statement in the content table then the content service will track
// //game variables in a database table?
//
// //or ask the map for the content and it will return some javascript to be executed
//
// //we'll have a content table that is just a guid and text content, one table for each language
// //we'll have a conditional table that will contain lua code that will produce the content id based on game state