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
    public class LocationEnterFailHandlerTests : InMemoryTest
    {
        #region Setup

        public LocationEnterFailHandlerTests() : base("LocationEnterFailHandlerTests")
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
        
        private LocationEnterFailHandler CreateHandler(ContentContext context)
        {
            var repository = new ContentRepository(context);
            var service = new ContentService(
                repository);
            return new LocationEnterFailHandler(service);
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
            //there should be a game with two content items
        }

        #endregion
    }
}