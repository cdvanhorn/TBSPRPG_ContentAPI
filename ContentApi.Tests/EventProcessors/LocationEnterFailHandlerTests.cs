using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentApi.Entities;
using ContentApi.EventProcessors;
using ContentApi.Repositories;
using ContentApi.Services;
using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;
using TbspRpgLib.Events.Location;
using Xunit;

namespace ContentApi.Tests.EventProcessors
{
    public class LocationEnterFailHandlerTests : InMemoryTest
    {
        #region Setup

        private readonly Guid _testContentId = Guid.NewGuid();
        public LocationEnterFailHandlerTests() : base("LocationEnterFailHandlerTests")
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
                Id = _testContentId,
                GameId = _testGameId,
                Position = 42,
                Text = "bananas"
            };

            context.Games.Add(game);
            context.Contents.Add(content);

            context.SaveChanges();
        }
        
        private LocationEnterFailHandler CreateHandler(ContentContext context)
        {
            var repository = new ContentRepository(context);
            var service = new ContentService(
                repository);
            var gameRepository = new GameRepository(context);
            var gameService = new GameService(gameRepository);
            return new LocationEnterFailHandler(service, gameService);
        }

        #endregion

        #region HandleEvent

        [Fact]
        public async void HandleEvent_NewContent_ContentAdded()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var handler = CreateHandler(context);
            var agg = new GameAggregate()
            {
                Id = _testGameId.ToString(),
                AdventureId = Guid.NewGuid().ToString()
            };
            
            var evnt = new LocationEnterFailEvent()
            {
                EventId = Guid.NewGuid(),
                StreamPosition = 43
            };
            
            //act
            await handler.HandleEvent(agg, evnt);
            
            //assert
            context.SaveChanges();
            //there should be a game with a content item
            Assert.Equal(2, context.Contents.AsQueryable().Count(c => c.GameId == _testGameId));
        }
        
        [Fact]
        public async void HandleEvent_ExistingContent_ContentNotAdded()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var handler = CreateHandler(context);
            var agg = new GameAggregate()
            {
                Id = _testGameId.ToString(),
                AdventureId = Guid.NewGuid().ToString()
            };
            
            var evnt = new LocationEnterFailEvent()
            {
                EventId = Guid.NewGuid(),
                StreamPosition = 42
            };
            
            //act
            await handler.HandleEvent(agg, evnt);
            
            //assert
            context.SaveChanges();
            //there should be a game with a content item
            Assert.Single(context.Contents.AsQueryable().Where(c => c.GameId == _testGameId));
        }
        
        [Fact]
        public async void HandleEvent_NoGame_ExceptionThrown()
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
            Task Act() => handler.HandleEvent(agg, null);

            //assert
            await Assert.ThrowsAsync<Exception>(Act);
        }

        #endregion
    }
}