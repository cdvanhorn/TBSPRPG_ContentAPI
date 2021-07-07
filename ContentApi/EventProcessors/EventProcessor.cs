// using System;
// using System.Threading.Tasks;
//
// using TbspRpgLib.EventProcessors;
// using TbspRpgLib.Aggregates;
// using TbspRpgLib.Settings;
// using TbspRpgLib.Events;
// using TbspRpgLib.Entities;
//
// using Microsoft.Extensions.DependencyInjection;
//
// using ContentApi.Repositories;
// using ContentApi.Services;
//
// namespace ContentApi.EventProcessors
// {
//     public class EventProcessor : MultiEventProcessor
//     {
//         private readonly IServiceScopeFactory _scopeFactory;
//
//         public EventProcessor(
//             IEventStoreSettings eventStoreSettings,
//             IServiceScopeFactory scopeFactory) :
//                 base(
//                     "game",
//                     new string[] {
//                         Event.GAME_NEW_EVENT_TYPE,
//                         Event.LOCATION_ENTER_FAIL_EVENT_TYPE,
//                         Event.LOCATION_ENTER_PASS_EVENT_TYPE
//                     },
//                     eventStoreSettings
//                 )
//         {
//             _scopeFactory = scopeFactory;
//             var scope = scopeFactory.CreateScope();
//             var context = scope.ServiceProvider.GetRequiredService<ContentContext>();
//             InitializeStartPosition(context);
//         }
//
//         protected override async Task HandleEvent(Aggregate aggregate, Event evnt) {
//             GameAggregate gameAggregate = (GameAggregate)aggregate;
//             EventType eventType = GetEventTypeByName(evnt.Type);
//
//             using(var scope = _scopeFactory.CreateScope()) {
//                 var context = scope.ServiceProvider.GetRequiredService<ContentContext>();
//                 var service = scope.ServiceProvider.GetRequiredService<IContentService>();
//                 
//                 var transaction = context.Database.BeginTransaction();
//                 try {
//                     //check if we've already processed the event
//                     if(await service.HasBeenProcessed(evnt.EventId))
//                         return;
//
//                     //figure out what handler to call based on event type
//                     IEventHandler handler = null;
//                     if(eventType.TypeName == Event.GAME_NEW_EVENT_TYPE)
//                         handler = scope.ServiceProvider.GetRequiredService<INewGameEventHandler>();
//                     else if(eventType.TypeName == Event.LOCATION_ENTER_PASS_EVENT_TYPE)
//                         handler = scope.ServiceProvider.GetRequiredService<ILocationEnterPassHandler>();
//                     else if(eventType.TypeName == Event.LOCATION_ENTER_FAIL_EVENT_TYPE)
//                         handler = scope.ServiceProvider.GetRequiredService<ILocationEnterFailHandler>();
//                     if(handler != null)
//                         await handler.HandleEvent(gameAggregate, evnt);
//
//                     //update the event type position and this event is processed
//                     await service.UpdatePosition(eventType.Id, gameAggregate.GlobalPosition);
//                     await service.EventProcessed(evnt.EventId);
//                     //save the changes
//                     context.SaveChanges();
//                     transaction.Commit();
//                 } catch (Exception) {
//                     transaction.Rollback();
//                     throw new Exception("event processor error");
//                 }
//             }
//         }
//     }
// }
