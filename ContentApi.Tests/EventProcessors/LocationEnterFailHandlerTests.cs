using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var gameRepository = new GameRepository(context);
            var gameService = new GameService(gameRepository);
            return new LocationEnterFailHandler(service, gameService);
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
                AdventureId = Guid.NewGuid().ToString()
            };
            
            //act
            await handler.HandleEvent(agg, null);
            
            //assert
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
                Id = _testGameId.ToString(),
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