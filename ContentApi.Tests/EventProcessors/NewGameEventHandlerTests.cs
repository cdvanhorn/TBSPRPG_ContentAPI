using System;
using System.Collections.Generic;
using System.Linq;
using ContentApi.EventProcessors;
using ContentApi.Repositories;
using ContentApi.Services;
using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;
using Xunit;

namespace ContentApi.Tests.EventProcessors
{
    public class NewGameEventHandlerTests : InMemoryTest
    {
        #region Setup

        public NewGameEventHandlerTests() : base("NewGameEventHandlerTests")
        {
            Seed();
        }
        
        private void Seed()
        {
            using var context = new ContentContext(_dbContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.SaveChanges();
        }
        
        private NewGameEventHandler CreateHandler(ContentContext context)
        {
            var repository = new ContentRepository(context);
            var service = new ContentService(
                repository);
            var gameRepository = new GameRepository(context);
            var gameService = new GameService(gameRepository);
            return new NewGameEventHandler(service, gameService);
        }

        #endregion

        #region HandleEvent

        [Fact]
        public async void HandleEvent_ContentEventCreated()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var handler = CreateHandler(context);
            var agg = new GameAggregate()
            {
                Id = _testGameId.ToString(),
                AdventureId = Guid.NewGuid().ToString()
            };
            
            //act
            await handler.HandleEvent(agg, null);
            
            //assert
            context.SaveChanges();
            //there should be a new game in the database
            Assert.NotNull(context.Games.FirstOrDefault(g => g.Id == _testGameId));
        }

        #endregion
    }
}