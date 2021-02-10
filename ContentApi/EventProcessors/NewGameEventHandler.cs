using System;
using System.Threading.Tasks;

using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;

namespace ContentApi.EventProcessors {
    public interface INewGameEventHandler : IEventHandler {

    }

    public class NewGameEventHandler : EventHandler, INewGameEventHandler {

        public NewGameEventHandler() : base() {
            
        }

        public async Task HandleEvent(GameAggregate gameAggregate, Event evnt) {
            //need to create a new event stream that will be the content for this game

        }
    }
}