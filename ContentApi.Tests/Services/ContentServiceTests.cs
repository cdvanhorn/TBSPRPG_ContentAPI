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

        private ContentService CreateService(ContentContext context)
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
            Assert.Equal(_testGameId, gameContents.Id);
            Assert.Equal(3, gameContents.Texts.Count);
            Assert.Equal(_contentOne, gameContents.Texts[0]);
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
            Assert.Equal(_testGameId, gameContents.Id);
            Assert.Single(gameContents.Texts);
            Assert.Equal(_contentLatest, gameContents.Texts.First());
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
            Assert.Equal(_testGameId, gameContents.Id);
            Assert.Equal(3, gameContents.Texts.Count);
            Assert.Equal(_contentOne, gameContents.Texts[0]);
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
            Assert.Equal(_testGameId, gameContents.Id);
            Assert.Equal(3, gameContents.Texts.Count);
            Assert.Equal(_contentOne, gameContents.Texts[0]);
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
            Assert.Equal(_testGameId, gameContents.Id);
            Assert.Single(gameContents.Texts);
            Assert.Equal(_contentLatest, gameContents.Texts[0]);
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
            Assert.Equal(_testGameId, gameContents.Id);
            Assert.Equal(2, gameContents.Texts.Count);
            Assert.Equal(_contentTwo, gameContents.Texts[0]);
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
            Assert.Equal(_testGameId, gameContents.Id);
            Assert.Equal(2, gameContents.Texts.Count);
            Assert.Equal(_contentOne, gameContents.Texts[0]);
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
            Assert.Equal(_testGameId, gameContents.Id);
            Assert.Equal(3, gameContents.Texts.Count);
            Assert.Equal(_contentLatest, gameContents.Texts[0]);
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
            Assert.Equal(_testGameId, gameContents.Id);
            Assert.Equal(2, gameContents.Texts.Count);
            Assert.Equal(_contentTwo, gameContents.Texts[0]);
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
            Assert.Equal(_testGameId, gameContents.Id);
            Assert.Equal(2, gameContents.Texts.Count);
            Assert.Equal(_contentLatest, gameContents.Texts[0]);
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
            Assert.Equal(_testGameId, gameContents.Id);
            Assert.Equal(2, gameContents.Texts.Count);
            Assert.Equal(_contentOne, gameContents.Texts[1]);
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
    }
}