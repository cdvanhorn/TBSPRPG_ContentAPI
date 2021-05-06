using System.Collections.Generic;
using System.Linq;
using ContentApi.Controllers;
using ContentApi.Repositories;
using ContentApi.Services;
using ContentApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using TbspRpgLib.Events;
using Xunit;

namespace ContentApi.Tests.Controllers
{
    public class ContentControllerTests : InMemoryTest
    {
        #region Setup

        public ContentControllerTests() : base("ContentControllerTests")
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

        private ContentController CreateController(ContentContext context, ICollection<Event> events, List<string> contents)
        {
            var repository = new ContentRepository(context);
            var service = new ContentService(
                repository,
                MockAggregateService(events, contents));
            return new ContentController(service);
        }

        #endregion
        
        #region GetContentForGame

        [Fact]
        public async void GetForGame_GetAllContent()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var contents = new List<string>();
            contents.AddRange(new string[] { "one", "two", "three", "four", "five" });
            var controller = CreateController(context, null, contents);
            
            //act
            var result = await controller.GetForGame(_testGameId);
            
            //assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var apiContents = okObjectResult.Value as ContentViewModel;
            Assert.NotNull(apiContents);
            Assert.Equal(5, apiContents.Texts.Count);
        }
        
        #endregion
        
        #region GetLatestForGame
        
        [Fact]
        public async void GetLatestForGame_GetLatestContent()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var contents = new List<string>();
            contents.AddRange(new string[] { "one", "two", "three", "four", "five" });
            var controller = CreateController(context, null, contents);
            
            //act
            var result = await controller.GetLatestForGame(_testGameId);
            
            //assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var apiContents = okObjectResult.Value as ContentViewModel;
            Assert.NotNull(apiContents);
            Assert.Equal(1, apiContents.Texts.Count);
            Assert.Equal("five", apiContents.Texts.First());
        }
        
        #endregion

        #region FilterContent

        [Fact]
        public async void FilterContent_Valid_GetRequestedContent()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var contents = new List<string>();
            contents.AddRange(new string[] { "one", "two", "three", "four", "five" });
            var controller = CreateController(context, null, contents);
            
            //act
            var result = await controller.FilterContent(_testGameId, new ContentFilterRequest()
            {
                Direction = "f",
                Start = 2,
                Count = 2
            });
            
            //assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var apiContents = okObjectResult.Value as ContentViewModel;
            Assert.NotNull(apiContents);
            Assert.Equal(2, apiContents.Texts.Count);
        }
        
        [Fact]
        public async void FilterContent_BadDirection_ReturnBadRequest()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var contents = new List<string>();
            contents.AddRange(new string[] { "one", "two", "three", "four", "five" });
            var controller = CreateController(context, null, contents);
            
            //act
            var result = await controller.FilterContent(_testGameId, new ContentFilterRequest()
            {
                Direction = "zebra",
                Start = 2,
                Count = 2
            });
            
            //assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        #endregion
    }
}