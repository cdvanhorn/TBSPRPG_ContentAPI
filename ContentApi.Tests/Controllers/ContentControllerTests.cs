using System;
using System.Collections.Generic;
using System.Linq;
using ContentApi.Controllers;
using ContentApi.Entities;
using ContentApi.Entities.LanguageSources;
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

        private readonly Guid _testContentId;
        private readonly Guid _testContentKey = Guid.NewGuid();
        private readonly Guid _testGameId = Guid.NewGuid();
        private const string _testEnglishText = "text in english";
        private readonly string _contentOne = "first content";
        private readonly string _contentTwo = "second content";
        private readonly string _contentLatest = "latest content";
        public ContentControllerTests() : base("ContentControllerTests")
        {
            _testContentId = Guid.NewGuid();
            Seed();
        }
        
        private void Seed()
        {
            using var context = new ContentContext(_dbContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var game = new Game()
            {
                Id = _testGameId,
                AdventureId = Guid.NewGuid(),
                Language = "en"
            };
            
            var enSource = new En()
            {
                Id = Guid.NewGuid(),
                ContentKey = _testContentKey,
                Text = _testEnglishText
            };
            
            var enSource2 = new En()
            {
                Id = Guid.NewGuid(),
                ContentKey = Guid.NewGuid(),
                Text = "other english text"
            };

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
            
            context.SourcesEn.AddRange(enSource, enSource2);
            context.Contents.AddRange(tc, tc2, tc3, tc4);
            context.Games.Add(game);
            context.SaveChanges();
        }

        private static ContentController CreateController(ContentContext context)
        {
            var repository = new ContentRepository(context);
            var sourceRepository = new SourceRepository(context);
            var conditionalSourceRepository = new ConditionalSourceRepository(context);
            var service = new ContentService(repository);
            var sourceService = new SourceService(sourceRepository, conditionalSourceRepository);
            var gameService = new GameService(new GameRepository(context));
            return new ContentController(service, sourceService, gameService);
        }

        #endregion
        
        #region GetContentForGame

        [Fact]
        public async void GetForGame_GetAllContent()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var controller = CreateController(context);
            
            //act
            var result = await controller.GetForGame(_testGameId);
            
            //assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var apiContents = okObjectResult.Value as ContentViewModel;
            Assert.NotNull(apiContents);
            Assert.Equal(3, apiContents.Texts.Count);
        }
        
        #endregion
        
        #region GetLatestForGame
        
        [Fact]
        public async void GetLatestForGame_GetLatestContent()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var controller = CreateController(context);
            
            //act
            var result = await controller.GetLatestForGame(_testGameId);
            
            //assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var apiContents = okObjectResult.Value as ContentViewModel;
            Assert.NotNull(apiContents);
            Assert.Single(apiContents.Texts);
            Assert.Equal(_contentLatest, apiContents.Texts.First());
        }
        
        #endregion

        #region FilterContent

        [Fact]
        public async void FilterContent_Valid_GetRequestedContent()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var controller = CreateController(context);
            
            //act
            var result = await controller.FilterContent(_testGameId, new ContentFilterRequest()
            {
                Direction = "f",
                Start = 1,
                Count = 1
            });
            
            //assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var apiContents = okObjectResult.Value as ContentViewModel;
            Assert.NotNull(apiContents);
            Assert.Single(apiContents.Texts);
            Assert.Equal(_contentTwo, apiContents.Texts.First());
        }
        
        [Fact]
        public async void FilterContent_BadDirection_ReturnBadRequest()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var controller = CreateController(context);
            
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
        
        #region GetSourceContent

        [Fact]
        public async void GetSourceContent_Valid_ReturnSource()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var controller = CreateController(context);
            
            //act
            var result = await controller.GetSourceContent("en", _testContentKey);

            //assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sourceViewModel = okObjectResult.Value as SourceViewModel;
            Assert.NotNull(sourceViewModel);
            Assert.Equal(_testContentKey, sourceViewModel.Id);
            Assert.Equal("en", sourceViewModel.Language);
            Assert.Equal(_testEnglishText, sourceViewModel.Source);
        }
        
        [Fact]
        public async void GetSourceContent_ValidGame_ReturnSource()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var controller = CreateController(context);
            
            //act
            var result = await controller.GetSourceContent(_testGameId, _testContentKey);

            //assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sourceViewModel = okObjectResult.Value as SourceViewModel;
            Assert.NotNull(sourceViewModel);
            Assert.Equal(_testContentKey, sourceViewModel.Id);
            Assert.Equal("en", sourceViewModel.Language);
            Assert.Equal(_testEnglishText, sourceViewModel.Source);
        }
        
        [Fact]
        public async void GetSourceContent_InValidLanguage_ReturnError()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var controller = CreateController(context);
            
            //act
            var result = await controller.GetSourceContent("eng", _testContentKey);

            //assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        
        [Fact]
        public async void GetSourceContent_InValidGame_ReturnError()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var controller = CreateController(context);
            
            //act
            var result = await controller.GetSourceContent(Guid.NewGuid(), _testContentKey);

            //assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        
        [Fact]
        public async void GetSourceContent_InValidKey_ReturnSourceError()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var controller = CreateController(context);
            var invalidKey = Guid.NewGuid();
            
            //act
            var result = await controller.GetSourceContent("en", invalidKey);

            //assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var sourceViewModel = okObjectResult.Value as SourceViewModel;
            Assert.NotNull(sourceViewModel);
            Assert.Equal(invalidKey, sourceViewModel.Id);
            Assert.Equal("en", sourceViewModel.Language);
            Assert.Equal($"invalid source key {invalidKey}", sourceViewModel.Source);
        }
        
        #endregion
    }
}