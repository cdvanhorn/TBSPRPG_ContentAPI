using System;
using System.Collections.Generic;
using System.Linq;
using ContentApi.Entities;
using ContentApi.EventProcessors;
using ContentApi.Repositories;
using ContentApi.Services;
using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;
using TbspRpgLib.Events.Game;
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

            var content = new Content()
            {
                GameId = _testGameId,
                Id = Guid.NewGuid(),
                Position = 42,
                Text = "game created"
            };

            context.Games.Add(game);
            context.Contents.Add(content);
            context.SaveChanges();
        }
        
        private NewGameEventHandler CreateHandler(ContentContext context)
        {
            var service = new ContentService(
                new ContentRepository(context));
            var gameService = new GameService(
                new GameRepository(context));
            var sourceService = new SourceService(
                new SourceRepository(context),
                new ConditionalSourceRepository(context));
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

            var evnt = new GameNewEvent()
            {
                EventId = Guid.NewGuid(),
                StreamPosition = 43
            };
            
            //act
            await handler.HandleEvent(agg, evnt);
            
            //assert
            context.SaveChanges();
            //there should be a new game in the database
            Assert.NotNull(context.Games.FirstOrDefault(g => g.Id.ToString() == agg.Id));
            Assert.NotNull(context.Contents.FirstOrDefault(c => c.GameId.ToString() == agg.Id));
        }
        
        [Fact]
        public async void HandleEvent_GameExists_GameNotCreatedContentCreated()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var handler = CreateHandler(context);
            var agg = new GameAggregate()
            {
                Id = _testGameId.ToString(),
                AdventureId = Guid.NewGuid().ToString()
            };
            
            var evnt = new GameNewEvent()
            {
                EventId = Guid.NewGuid(),
                StreamPosition = 43
            };
            
            //act
            await handler.HandleEvent(agg, evnt);
            
            //assert
            context.SaveChanges();
            //there should be a new game in the database
            Assert.Single(context.Games.AsQueryable().Where(g => g.Id == _testGameId));
            Assert.Equal(2, context.Contents.AsQueryable().Count(c => c.GameId.ToString() == agg.Id));
        }
        
        [Fact]
        public async void HandleEvent_GameContentExists_GameContentNotCreated()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var handler = CreateHandler(context);
            var agg = new GameAggregate()
            {
                Id = _testGameId.ToString(),
                AdventureId = Guid.NewGuid().ToString()
            };
            
            var evnt = new GameNewEvent()
            {
                EventId = Guid.NewGuid(),
                StreamPosition = 42
            };
            
            //act
            await handler.HandleEvent(agg, evnt);
            
            //assert
            context.SaveChanges();
            //there should be a new game in the database
            Assert.Single(context.Games.AsQueryable().Where(g => g.Id == _testGameId));
            Assert.Single(context.Contents.AsQueryable().Where(c => c.GameId.ToString() == agg.Id));
        }

        #endregion
    }
}