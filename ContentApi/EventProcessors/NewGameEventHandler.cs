// using System;
// using System.Threading.Tasks;
// using ContentApi.Entities;
// using ContentApi.Services;
// using TbspRpgLib.Aggregates;
// using TbspRpgLib.Events;
// using TbspRpgLib.Events.Content;
//
// namespace ContentApi.EventProcessors {
//     public interface INewGameEventHandler : IEventHandler {
//
//     }
//
//     public class NewGameEventHandler : EventHandler, INewGameEventHandler {
//
//         public NewGameEventHandler(IContentService contentService, ISourceService sourceService, IGameService gameService) :
//             base(contentService, sourceService, gameService) {
//         }
//
//         protected override async Task HandleEvent(Event evnt)
//         {
//             await _gameService.AddGame(_game);
//             var content = new Content()
//             {
//                 GameId = _game.Id,
//                 Position = evnt.StreamPosition,
//                 Text = await _sourceService.GetSourceForKey(_game.AdventureId)
//             };
//             await _contentService.AddContent(content);
//         }
//     }
// }