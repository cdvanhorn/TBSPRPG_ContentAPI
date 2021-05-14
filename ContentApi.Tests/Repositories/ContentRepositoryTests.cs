using System;
using System.Linq;
using ContentApi.Entities;
using ContentApi.Repositories;
using Xunit;

namespace ContentApi.Tests.Repositories
{
    public class ContentRepositoryTests : InMemoryTest
    {
        #region Setup

        private readonly Guid _testContentId;
        
        public ContentRepositoryTests() : base("ContentRepositoryTests")
        {
            _testContentId = Guid.NewGuid();
            Seed();
        }

        private void Seed()
        {
            using var context = new ContentContext(_dbContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var tc = new Content()
            {
                Id = _testContentId,
                GameId = _testGameId,
                Position = 42,
                Text = "latest content"
            };

            var tc2 = new Content()
            {
                Id = Guid.NewGuid(),
                GameId = _testGameId,
                Position = 0,
                Text = "first content"
            };

            var tc3 = new Content()
            {
                Id = Guid.NewGuid(),
                GameId = _testGameId,
                Position = 1,
                Text = "second content"
            };

            var tc4 = new Content()
            {
                Id = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                Position = 43,
                Text = "other game content"
            };
            
            context.Contents.AddRange(tc, tc2, tc3, tc4);
            context.SaveChanges();
        }

        #endregion
        
        #region GetContentForGame

        [Fact]
        public async void GetContentForGame_NoCountNoOffset_ReturnsAll()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var repository = new ContentRepository(context);
            
            //act
            var contents = await repository.GetContentForGame(_testGameId);
            
            //assert
            Assert.Equal(3, contents.Count);
            Assert.Equal((ulong)0, contents[0].Position);
            Assert.Equal((ulong)42, contents[2].Position);
        }
        
        [Fact]
        public async void GetContentForGame_NoOffset_ReturnPartial()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var repository = new ContentRepository(context);
            
            //act
            var contents = await repository.GetContentForGame(_testGameId, null, 2);
            
            //assert
            Assert.Equal(2, contents.Count);
            Assert.Equal((ulong)0, contents[0].Position);
            Assert.Equal((ulong)1, contents[1].Position);
        }

        [Fact]
        public async void GetContentForGame_NoCount_ReturnPartial()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var repository = new ContentRepository(context);
            
            //act
            var contents = await repository.GetContentForGame(_testGameId, 2);
            
            //assert
            Assert.Single(contents);
            Assert.Equal(_testContentId, contents[0].Id);
            Assert.Equal((ulong)42, contents[0].Position);
        }
        
        [Fact]
        public async void GetContentForGame_OffsetCount_ReturnPartial()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var repository = new ContentRepository(context);
            
            //act
            var contents = await repository.GetContentForGame(_testGameId, 1, 2);
            
            //assert
            Assert.Equal(2, contents.Count);
            Assert.Equal((ulong)1, contents[0].Position);
            Assert.Equal((ulong)42, contents[1].Position);
        }
        
        #endregion
        
        #region GetContentForGameReverse

        [Fact]
        public async void GetContentForGameReverse_NoCountNoOffset_ReturnsAll()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var repository = new ContentRepository(context);
            
            //act
            var contents = await repository.GetContentForGameReverse(_testGameId);
            
            //assert
            Assert.Equal(3, contents.Count);
            Assert.Equal((ulong)42, contents[0].Position);
            Assert.Equal((ulong)0, contents[2].Position);
        }
        
        [Fact]
        public async void GetContentForGameReverse_NoOffset_ReturnPartial()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var repository = new ContentRepository(context);
            
            //act
            var contents = await repository.GetContentForGameReverse(_testGameId, null, 2);
            
            //assert
            Assert.Equal(2, contents.Count);
            Assert.Equal((ulong)42, contents[0].Position);
            Assert.Equal((ulong)1, contents[1].Position);
        }

        [Fact]
        public async void GetContentForGameReverse_NoCount_ReturnPartial()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var repository = new ContentRepository(context);
            
            //act
            var contents = await repository.GetContentForGameReverse(_testGameId, 2);
            
            //assert
            Assert.Single(contents);
            Assert.Equal((ulong)0, contents[0].Position);
        }
        
        [Fact]
        public async void GetContentForGameReverse_OffsetCount_ReturnPartial()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var repository = new ContentRepository(context);
            
            //act
            var contents = await repository.GetContentForGameReverse(_testGameId, 1, 2);
            
            //assert
            Assert.Equal(2, contents.Count);
            Assert.Equal((ulong)1, contents[0].Position);
            Assert.Equal((ulong)0, contents[1].Position);
        }
        
        #endregion

        #region AddContent

        [Fact]
        public async void AddContent_Valid_ContentAdded()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var repository = new ContentRepository(context);
            var content = new Content()
            {
                GameId = _testGameId,
                Position = 43,
                Text = "add content"
            };
            
            //act
            repository.AddContent(content);
            repository.SaveChanges();
            
            //assert
            Assert.Equal(5, context.Contents.Count());
            Assert.NotNull(context.Contents.FirstOrDefault(c => c.Id == content.Id));
        }

        #endregion
    }
}