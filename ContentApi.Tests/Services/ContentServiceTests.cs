using System;
using System.Collections.Generic;
using System.Linq;
using ContentApi.Entities;
using ContentApi.Repositories;
using ContentApi.Services;
using ContentApi.ViewModels;
using TbspRpgLib.Events;
using Xunit;

namespace ContentApi.Tests.Services
{
    public class ContentServiceTests : InMemoryTest
    {
        #region Setup
        
        private readonly Guid _testContentId;
        private readonly string _contentOne = "first content";
        private readonly string _contentTwo = "second content";
        private readonly string _contentLatest = "latest content";
        public ContentServiceTests() : base("ContentServiceTests")
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
                Text = _contentLatest
            };

            var tc2 = new Content()
            {
                Id = Guid.NewGuid(),
                GameId = _testGameId,
                Position = 0,
                Text = _contentOne
            };

            var tc3 = new Content()
            {
                Id = Guid.NewGuid(),
                GameId = _testGameId,
                Position = 1,
                Text = _contentTwo
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

        private static ContentService CreateService(ContentContext context)
        {
            var repository = new ContentRepository(context);
            return new ContentService(repository);
        }
        
        #endregion

        #region GetAllContent

        [Fact]
        public async void GetAllContentForGame_GetsAllContent()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetAllContentForGame(_testGameId);
            
            //assert
            Assert.Equal(_testGameId, gameContents.FirstOrDefault().GameId);
            Assert.Equal(3, gameContents.Count);
            Assert.Equal(_contentOne, gameContents.FirstOrDefault().Text);
        }

        #endregion

        #region GetLatestForGame

        [Fact]
        public async void GetLatestForGame_GetsLatest()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetLatestForGame(_testGameId);
            
            //assert
            Assert.Equal(_testGameId, gameContents.GameId);
            Assert.Equal(_contentLatest, gameContents.Text);
        }

        #endregion
        
        #region GetPartialContentForGame

        [Fact]
        public async void GetPartialContentForGame_NoDirection_ContentsForward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(_testGameId, new ContentFilterRequest());
            
            //assert
            Assert.Equal(_testGameId, gameContents.FirstOrDefault().GameId);
            Assert.Equal(3, gameContents.Count);
            Assert.Equal(_contentOne, gameContents.FirstOrDefault().Text);
        }
        
        [Fact]
        public async void GetPartialContentForGame_Forward_ContentsForward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(_testGameId, new ContentFilterRequest()
            {
                Direction = "f"
            });
            
            //assert
            Assert.Equal(_testGameId, gameContents.FirstOrDefault().GameId);
            Assert.Equal(3, gameContents.Count);
            Assert.Equal(_contentOne, gameContents.FirstOrDefault().Text);
        }
        
        [Fact]
        public async void GetPartialContentForGame_ForwardStart_PartialContentsForward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(_testGameId, new ContentFilterRequest()
            {
                Direction = "f",
                Start = 2
            });
            
            //assert
            Assert.Equal(_testGameId, gameContents.FirstOrDefault().GameId);
            Assert.Single(gameContents);
            Assert.Equal(_contentLatest, gameContents.FirstOrDefault().Text);
        }
        
        [Fact]
        public async void GetPartialContentForGame_ForwardCountStart_PartialContentsForward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(_testGameId, new ContentFilterRequest()
            {
                Direction = "f",
                Start = 1,
                Count = 2
            });
            
            //assert
            Assert.Equal(_testGameId, gameContents.FirstOrDefault().GameId);
            Assert.Equal(2, gameContents.Count);
            Assert.Equal(_contentTwo, gameContents.FirstOrDefault().Text);
        }
        
        [Fact]
        public async void GetPartialContentForGame_ForwardCount_PartialContentsForward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(_testGameId, new ContentFilterRequest()
            {
                Direction = "f",
                Count = 2
            });
            
            //assert
            Assert.Equal(_testGameId, gameContents.FirstOrDefault().GameId);
            Assert.Equal(2, gameContents.Count);
            Assert.Equal(_contentOne, gameContents.FirstOrDefault().Text);
        }
        
        [Fact]
        public async void GetPartialContentForGame_Backward_ContentsBackward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(_testGameId, new ContentFilterRequest()
            {
                Direction = "b"
            });
            
            //assert
            Assert.Equal(_testGameId, gameContents.FirstOrDefault().GameId);
            Assert.Equal(3, gameContents.Count);
            Assert.Equal(_contentLatest, gameContents.FirstOrDefault().Text);
        }
        
        [Fact]
        public async void GetPartialContentForGame_BackwardStart_PartialContentsBackward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(_testGameId, new ContentFilterRequest()
            {
                Direction = "b",
                Start = 1
            });
            
            //assert
            Assert.Equal(_testGameId, gameContents.FirstOrDefault().GameId);
            Assert.Equal(2, gameContents.Count);
            Assert.Equal(_contentTwo, gameContents.FirstOrDefault().Text);
        }
        
        [Fact]
        public async void GetPartialContentForGame_BackwardCount_PartialContentsBackward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(_testGameId, new ContentFilterRequest()
            {
                Direction = "b",
                Count = 2
            });
            
            //assert
            Assert.Equal(_testGameId, gameContents.FirstOrDefault().GameId);
            Assert.Equal(2, gameContents.Count);
            Assert.Equal(_contentLatest, gameContents.FirstOrDefault().Text);
        }
        
        [Fact]
        public async void GetPartialContentForGame_BackwardStartCount_PartialContentsBackward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var gameContents = await service.GetPartialContentForGame(_testGameId, new ContentFilterRequest()
            {
                Direction = "b",
                Start = 1,
                Count = 2
            });
            
            //assert
            Assert.Equal(_testGameId, gameContents.FirstOrDefault().GameId);
            Assert.Equal(2, gameContents.Count);
            Assert.Equal(_contentOne, gameContents[1].Text);
        }
        
        [Fact]
        public async void GetPartialContentForGame_BadDirection_Error()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);

            //act
            //assert
            var exception = Assert.ThrowsAsync<ArgumentException>(() =>
                service.GetPartialContentForGame(_testGameId, new ContentFilterRequest()
                {
                    Direction = "zebra",
                    Start = -3,
                    Count = 2
                }));
        }
        
        #endregion

        #region AddContent

        [Fact]
        public async void AddContent_NotExists_ContentAdded()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            var content = new Content()
            {
                Id = Guid.NewGuid(),
                GameId = _testGameId,
                Position = 43,
                Text = "added content"
            };
            
            //act
            await service.AddContent(content);
            
            //assert
            context.SaveChanges();
            Assert.Equal(4, context.Contents.Count(c => c.GameId == _testGameId));
            Assert.NotNull(context.Contents.FirstOrDefault(c => c.Id == content.Id));
        }
        
        [Fact]
        public async void AddContent_Exists_ContentNotAdded()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            var content = new Content()
            {
                Id = Guid.NewGuid(),
                GameId = _testGameId,
                Position = 42,
                Text = "added content"
            };
            
            //act
            await service.AddContent(content);
            
            //assert
            context.SaveChanges();
            Assert.Equal(3, context.Contents.Count(c => c.GameId == _testGameId));
            Assert.Null(context.Contents.FirstOrDefault(c => c.Id == content.Id));
        }

        #endregion
        
        #region GetContentForGameAfterPosition

        [Fact]
        public async void GetContentForGameAfterPosition_EarlyPosition_ReturnContent()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var contents = await service.GetContentForGameAfterPosition(_testGameId, 40);
            
            //assert
            Assert.Single(contents);
            Assert.Equal(_contentLatest, contents[0].Text);
        }
        
        [Fact]
        public async void GetContentForGameAfterPosition_LastPosition_ReturnNoContent()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var contents = await service.GetContentForGameAfterPosition(_testGameId, 42);
            
            //assert
            Assert.Empty(contents);
        }
        
        #endregion
    }
}