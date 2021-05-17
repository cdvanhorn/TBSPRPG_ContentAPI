using System;
using System.Collections.Generic;
using System.Linq;
using ContentApi.Entities;
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

            var game = new Game()
            {
                Id = _testGameId
            };

            context.Games.Add(game);
            context.SaveChanges();
        }
        
        private NewGameEventHandler CreateHandler(ContentContext context)
        {
            var repository = new ContentRepository(context);
            var service = new ContentService(
                repository);
            var gameRepository = new GameRepository(context);
            var gameService = new GameService(gameRepository);
            var sourceRepository = new SourceRepository(context);
            var sourceService = new SourceService(sourceRepository);
            return new NewGameEventHandler(service, gameService, sourceService);
        }

        #endregion

        #region HandleEvent

        [Fact]
        public async void HandleEvent_DoesntExist_GameCreated()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var handler = CreateHandler(context);
            var agg = new GameAggregate()
            {
                Id = Guid.NewGuid().ToString(),
                AdventureId = Guid.NewGuid().ToString()
            };
            
            //act
            await handler.HandleEvent(agg, null);
            
            //assert
            context.SaveChanges();
            //there should be a new game in the database
            Assert.NotNull(context.Games.FirstOrDefault(g => g.Id.ToString() == agg.Id));
        }
        
        [Fact]
        public async void HandleEvent_Exists_GameNotCreated()
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
            Assert.Single(context.Games.AsQueryable().Where(g => g.Id == _testGameId));
        }

        #endregion
    }
}