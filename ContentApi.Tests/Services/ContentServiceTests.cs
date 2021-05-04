using System;
using System.Collections.Generic;
using System.Linq;
using ContentApi.Repositories;
using ContentApi.Services;
using TbspRpgLib.Events;
using Xunit;

namespace ContentApi.Tests.Services
{
    public class ContentServiceTests : InMemoryTest
    {
        #region Setup
        
        public ContentServiceTests() : base("ContentServiceTests")
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

        private ContentService CreateService(ContentContext context, ICollection<Event> events, List<string> contents)
        {
            var repository = new ContentRepository(context);
            return new ContentService(
                repository,
                MockAggregateService(events, contents));
        }
        
        #endregion

        #region GetAllContent

        [Fact]
        public async void GetAllContentForGame_GetsAllContent()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var contents = new List<string>();
            contents.AddRange(new string[] { "one", "two", "three", "four", "five" });
            var service = CreateService(context, null, contents);
            
            //act
            var gameContents = await service.GetAllContentForGame(_testGameId);
            
            //assert
            Assert.Equal(_testGameId.ToString(), gameContents.Id);
            Assert.Equal(5, gameContents.Texts.Count);
            Assert.Equal("one", gameContents.Texts[0]);
        }

        #endregion

        #region GetLatestForGame

        [Fact]
        public async void GetLatestForGame_GetsLatest()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var contents = new List<string>();
            contents.AddRange(new string[] { "one", "two", "three", "four", "five" });
            var service = CreateService(context, null, contents);
            
            //act
            var gameContents = await service.GetLatestForGame(_testGameId);
            
            //assert
            Assert.Equal(_testGameId.ToString(), gameContents.Id);
            Assert.Single(gameContents.Texts);
            Assert.Equal("five", gameContents.Texts.First());
        }

        #endregion
        
        #region GetPartialContentForGame

        [Fact]
        public async void GetPartialContentForGame_NoDirection_ContentsForward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var contents = new List<string>();
            contents.AddRange(new string[] { "one", "two", "three", "four", "five" });
            var service = CreateService(context, null, contents);
        }
        
        [Fact]
        public async void GetPartialContentForGame_Forward_ContentsForward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var contents = new List<string>();
            contents.AddRange(new string[] { "one", "two", "three", "four", "five" });
            var service = CreateService(context, null, contents);
        }
        
        [Fact]
        public async void GetPartialContentForGame_ForwardStart_PartialContentsForward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var contents = new List<string>();
            contents.AddRange(new string[] { "one", "two", "three", "four", "five" });
            var service = CreateService(context, null, contents);
        }
        
        [Fact]
        public async void GetPartialContentForGame_ForwardCountStart_PartialContentsForward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var contents = new List<string>();
            contents.AddRange(new string[] { "one", "two", "three", "four", "five" });
            var service = CreateService(context, null, contents);
        }
        
        [Fact]
        public async void GetPartialContentForGame_ForwardCount_PartialContentsForward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var contents = new List<string>();
            contents.AddRange(new string[] { "one", "two", "three", "four", "five" });
            var service = CreateService(context, null, contents);
        }
        
        [Fact]
        public async void GetPartialContentForGame_Backward_ContentsBackward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var contents = new List<string>();
            contents.AddRange(new string[] { "one", "two", "three", "four", "five" });
            var service = CreateService(context, null, contents);
        }
        
        [Fact]
        public async void GetPartialContentForGame_BackwardStart_PartialContentsBackward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var contents = new List<string>();
            contents.AddRange(new string[] { "one", "two", "three", "four", "five" });
            var service = CreateService(context, null, contents);
        }
        
        [Fact]
        public async void GetPartialContentForGame_BackwardCount_PartialContentsBackward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var contents = new List<string>();
            contents.AddRange(new string[] { "one", "two", "three", "four", "five" });
            var service = CreateService(context, null, contents);
        }
        
        [Fact]
        public async void GetPartialContentForGame_BackwardStartCount_PartialContentsBackward()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var contents = new List<string>();
            contents.AddRange(new string[] { "one", "two", "three", "four", "five" });
            var service = CreateService(context, null, contents);
        }
        
        [Fact]
        public async void GetPartialContentForGame_BadDirection_Error()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var contents = new List<string>();
            contents.AddRange(new string[] { "one", "two", "three", "four", "five" });
            var service = CreateService(context, null, contents);
        }
        
        #endregion
    }
}