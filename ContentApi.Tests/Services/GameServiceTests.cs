using System;
using System.Linq;
using ContentApi.Entities;
using ContentApi.Repositories;
using ContentApi.Services;
using Xunit;

namespace ContentApi.Tests.Services
{
    public class GameServiceTests : InMemoryTest
    {
        #region Setup

        private readonly Guid _testGameId;

        public GameServiceTests() : base("GameServiceTests")
        {
            _testGameId = Guid.NewGuid();
            Seed();
        }

        private void Seed()
        {
            using var context = new ContentContext(_dbContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            var testGame = new Game()
            {
                Id = _testGameId
            };

            var testGame2 = new Game()
            {
                Id = Guid.NewGuid()
            };
            
            context.Games.AddRange(testGame, testGame2);
            context.SaveChanges();
        }

        private static GameService CreateService(ContentContext context)
        {
            var repository = new GameRepository(context);
            return new GameService(repository);
        }
        #endregion

        #region GetGameById

        [Fact]
        public async void GetGameById_IdExists_ReturnOne()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var game = await service.GetGameById(_testGameId);
            
            //assert
            Assert.NotNull(game);
            Assert.Equal(_testGameId, game.Id);
        }
        
        [Fact]
        public async void GetGameById_IdDoesNotExists_ReturnNone()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var game = await service.GetGameById(Guid.NewGuid());
            
            //assert
            Assert.Null(game);
        }

        #endregion

        #region AddGame

        [Fact]
        public async void AddGame_DoesNotExist_GameAdded()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            var newGame = new Game()
            {
                Id = Guid.NewGuid()
            };
            
            //act
            await service.AddGame(newGame);
            
            //assert
            context.SaveChanges();
            Assert.Equal(3, context.Games.Count());
            Assert.NotNull(context.Games.FirstOrDefault(g => g.Id == newGame.Id));
        }

        [Fact]
        public async void AddGame_Exists_GameNotAdded()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            var newGame = new Game()
            {
                Id = _testGameId
            };
            
            //act
            await service.AddGame(newGame);
            
            //assert
            context.SaveChanges();
            Assert.Equal(2, context.Games.Count());
            Assert.NotNull(context.Games.FirstOrDefault(g => g.Id == newGame.Id));
        }

        #endregion
    }
}