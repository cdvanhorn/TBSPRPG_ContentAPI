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
        
        private LocationEnterFailHandler CreateHandler(ContentContext context, ICollection<Event> events, List<string> contents)
        {
            // var repository = new ContentRepository(context);
            // var service = new ContentService(
            //     repository,
            //     MockAggregateService(events, contents));
            return new LocationEnterFailHandler(MockAggregateService(events, contents));
        }

        #endregion

        #region HandleEvent

        [Fact]
        public async void HandleEvent_ContentEventCreated()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var events = new List<Event>();
            var handler = CreateHandler(context, events, null);
            var agg = new GameAggregate()
            {
                Id = _testGameId.ToString(),
                AdventureId = Guid.NewGuid().ToString()
            };
            
            //act
            await handler.HandleEvent(agg, null);
            
            //assert
            Assert.Single(events);
            var evnt = events.First();
            Assert.Equal(Event.CONTENT_EVENT_TYPE, evnt.Type);
        }

        #endregion
    }
}