// using System;
// using TbspRpgLib.Events;
// using TbspRpgLib.Aggregates;
//
// using System.Threading.Tasks;
// using ContentApi.Entities;
// using ContentApi.Services;
//
// namespace ContentApi.EventProcessors {
//     public interface ILocationEnterFailHandler : IEventHandler {
//
//     }
//     public class LocationEnterFailHandler : EventHandler, ILocationEnterFailHandler {
//         public LocationEnterFailHandler(IContentService contentService, ISourceService sourceService, IGameService gameService) :
//             base(contentService, sourceService, gameService) {
//         }
//
//         protected override async Task HandleEvent(Event evnt) {
//             //add some content to the database
//             var content = new Content()
//             {
//                 GameId = _game.Id,
//                 Position = evnt.StreamPosition,
//                 Text = $"{_game.Id} unsuccessfully entered a location"
//             };
//             await _contentService.AddContent(content);
//         }
//     }
// }