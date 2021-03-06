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
using TbspRpgLib.Settings;
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

            var content = new Content()
            {
                GameId = _testGameId,
                Id = Guid.NewGuid(),
                Position = 42,
                Text = "game created"
            };

            context.Contents.Add(content);
            context.SaveChanges();
        }
        
        private NewGameEventHandler CreateHandler(ContentContext context)
        {
            var service = new ContentService(
                new ContentRepository(context));
            var sourceService = new SourceService(
                new SourceRepository(context),
                new ConditionalSourceRepository(context));
            var gameService = new GameService(
                new GameRepository(context));
            return new NewGameEventHandler(service, sourceService, gameService);
        }

        #endregion

        #region HandleEvent

        [Fact]
        public async void HandleEvent_ContentCreated()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var handler = CreateHandler(context);
            var agg = new GameAggregate()
            {
                Id = _testGameId.ToString(),
                AdventureId = Guid.NewGuid().ToString(),
                Settings = new Settings()
                {
                    Language = Languages.ENGLISH
                }
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
            Assert.Equal(2, context.Contents.AsQueryable().Count(c => c.GameId.ToString() == agg.Id));
            Assert.Single(context.Games);
            Assert.Equal(Languages.ENGLISH, context.Games.First().Language);
        }
        
        [Fact]
        public async void HandleEvent_GameContentExists_GameContentNotCreated()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var handler = CreateHandler(context);
            var adventureId = Guid.NewGuid();
            context.Games.Add(new Game()
            {
                Id = _testGameId,
                AdventureId = adventureId
            });
            context.SaveChanges();
            var agg = new GameAggregate()
            {
                Id = _testGameId.ToString(),
                AdventureId = adventureId.ToString()
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
            Assert.Single(context.Contents.AsQueryable().Where(c => c.GameId.ToString() == agg.Id));
            Assert.Single(context.Games);
        }

        #endregion
    }
}