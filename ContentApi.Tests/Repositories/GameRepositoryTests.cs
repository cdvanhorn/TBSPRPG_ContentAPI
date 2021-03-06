using System;
using System.Linq;
using ContentApi.Entities;
using ContentApi.Repositories;
using Xunit;

namespace ContentApi.Tests.Repositories
{
    public class GameRepositoryTests : InMemoryTest
    {
        #region Setup

        private readonly Guid _testGameId;
        
        public GameRepositoryTests() : base("GameRepositoryTests")
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
                Id = _testGameId,
                Language = "en"
            };

            var testGame2 = new Game()
            {
                Id = Guid.NewGuid()
            };
            
            context.Games.AddRange(testGame, testGame2);
            context.SaveChanges();
        }

        #endregion

        #region GetAllGames

        [Fact]
        public async void GetAllGames_ReturnsAllGames()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var repository = new GameRepository(context);
            
            //act
            var games = await repository.GetAllGames();

            //assert
            Assert.Equal(2, games.Count);
            Assert.Equal(_testGameId, games.First().Id);
            Assert.Equal("en", games.First().Language);
        }

        #endregion

        #region GetGameById

        [Fact]
        public async void GetGameById_Valid_ReturnsOne()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var repository = new GameRepository(context);
            
            //act
            var game = await repository.GetGameById(_testGameId);

            //assert
            Assert.NotNull(game);
            Assert.Equal(_testGameId, game.Id);
        }
        
        [Fact]
        public async void GetGameById_BadId_ReturnsNone()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var repository = new GameRepository(context);
            
            //act
            var game = await repository.GetGameById(Guid.NewGuid());

            //assert
            Assert.Null(game);
        }

        #endregion

        #region AddGame

        [Fact]
        public async void AddGame_Valid_GameAdded()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var repository = new GameRepository(context);
            var newGame = new Game()
            {
                Id = Guid.NewGuid()
            };

            //act
            repository.AddGame(newGame);
            repository.SaveChanges();
            
            //assert
            Assert.Equal(3, context.Games.Count());
            Assert.NotNull(context.Games.FirstOrDefault(g => g.Id == newGame.Id));
        }

        #endregion
    }
}