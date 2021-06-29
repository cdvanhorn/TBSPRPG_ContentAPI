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

            var content = new Content()
            {
                Id = _testContentId,
                GameId = _testGameId,
                Position = 42,
                Text = "bananas"
            };

            context.Contents.Add(content);

            context.SaveChanges();
        }
        
        private LocationEnterFailHandler CreateHandler(ContentContext context)
        {
            var service = new ContentService(
                new ContentRepository(context));
            var sourceService = new SourceService(
                new SourceRepository(context),
                new ConditionalSourceRepository(context));
            var gameService = new GameService(
                new GameRepository(context));
            return new LocationEnterFailHandler(service, sourceService, gameService);
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
                AdventureId = Guid.NewGuid().ToString(),
                MapData = new MapData()
                {
                    CurrentLocation = Guid.NewGuid().ToString()
                }
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
                AdventureId = Guid.NewGuid().ToString(),
                MapData = new MapData()
                {
                    CurrentLocation = Guid.NewGuid().ToString()
                }
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

        #endregion
    }
}