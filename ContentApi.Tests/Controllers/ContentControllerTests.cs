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

        private readonly Guid _testContentKey = Guid.NewGuid();
        //private readonly Guid _testGameId = Guid.NewGuid();
        private const string TestEnglishText = "text in english";
        public ContentControllerTests() : base("ContentControllerTests")
        {
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
                Text = TestEnglishText
            };
            
            var enSource2 = new En()
            {
                Id = Guid.NewGuid(),
                ContentKey = Guid.NewGuid(),
                Text = "other english text"
            };

            context.SourcesEn.AddRange(enSource, enSource2);
            context.Games.Add(game);
            context.SaveChanges();
        }

        private static ContentController CreateController(ContentContext context)
        {
            var sourceRepository = new SourceRepository(context);
            var conditionalSourceRepository = new ConditionalSourceRepository(context);
            var sourceService = new SourceService(sourceRepository, conditionalSourceRepository);
            return new ContentController(sourceService);
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
            Assert.Equal(TestEnglishText, sourceViewModel.Source);
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