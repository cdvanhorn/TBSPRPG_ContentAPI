using System;
using System.Threading.Tasks;
using ContentApi.Entities;
using ContentApi.Entities.LanguageSources;
using ContentApi.Repositories;
using ContentApi.Services;
using TbspRpgLib.Settings;
using Xunit;

namespace ContentApi.Tests.Services
{
    public class SourceServiceTests : InMemoryTest
    {
        #region Setup
        
        private readonly Guid _testContentKey = Guid.NewGuid();
        private readonly Guid _testJavaScriptKey = Guid.NewGuid();
        private readonly Guid _testBadJavaScriptKey = Guid.NewGuid();
        private readonly Guid _testBadJavaScriptReturnKey = Guid.NewGuid();
        private const string _testEnglishText = "text in english";
        private const string _testSpanishText = "text in spanish";

        public SourceServiceTests() : base("SourceServiceTests")
        {
            Seed();
        }

        private void Seed()
        {
            using var context = new ContentContext(_dbContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

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

            var espSource = new Esp()
            {
                Id = Guid.NewGuid(),
                ContentKey = _testContentKey,
                Text = _testSpanishText
            };

            var conditionalSource = new ConditionalSource()
            {
                ContentKey = _testJavaScriptKey,
                Id = Guid.NewGuid(),
                JavaScript = $"function eval() {{ return '{_testContentKey}'; }}"
            };
            
            var conditionalSource2 = new ConditionalSource()
            {
                ContentKey = _testJavaScriptKey,
                Id = Guid.NewGuid(),
                JavaScript = $"function eval() {{ return gameState.AdventureId; }}"
            };
            
            var badConditionalSource = new ConditionalSource()
            {
                ContentKey = _testBadJavaScriptKey,
                Id = Guid.NewGuid(),
                JavaScript = $"functin eval() {{ return '{_testContentKey}'; }}"
            };
            
            var badConditionalSource2 = new ConditionalSource()
            {
                ContentKey = _testBadJavaScriptReturnKey,
                Id = Guid.NewGuid(),
                JavaScript = $"function eval() {{ return 'banana'; }}"
            };
            
            context.SourcesEn.AddRange(enSource, enSource2);
            context.SourcesEsp.Add(espSource);
            context.ConditionalSources.AddRange(
                conditionalSource,
                conditionalSource2,
                badConditionalSource,
                badConditionalSource2);
            context.SaveChanges();
        }

        private static SourceService CreateService(ContentContext context)
        {
            return new SourceService(
                new SourceRepository(context),
                new ConditionalSourceRepository(context));
        }
        
        #endregion
        
        #region GetSourceForKey

        [Fact]
        public async void GetSourceForKey_Valid_SourceReturned()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var text = await service.GetSourceForKey(_testContentKey);
            
            //assert
            Assert.Equal(_testEnglishText, text);
        }

        [Fact]
        public async void GetSourceForKey_InvalidKey_ReturnErrorText()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var badGuid = Guid.NewGuid();
            var text = await service.GetSourceForKey(badGuid);
            
            //assert
            Assert.Equal($"invalid source key {badGuid}", text);
        }

        [Fact]
        public async void GetSourceForKey_ChangeLanguage_SourceReturnedInLanguage()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var text = await service.GetSourceForKey(_testContentKey, null, Languages.SPANISH);
            
            //assert
            Assert.Equal(_testSpanishText, text);
        }
        
        [Fact]
        public async void GetSourceForKey_InvalidLanguage_ThrowException()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);

            //act
            Task Act() => service.GetSourceForKey(_testContentKey, null, "banana");

            //assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Fact]
        public async void GetSourceForKey_ValidJavaScript_ReturnSource()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var text = await service.GetSourceForKey(_testJavaScriptKey);
            
            //assert
            Assert.Equal(_testEnglishText, text);
        }
        
        [Fact]
        public async void GetSourceForKey_ValidJavaScriptGameState_ReturnSource()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            var game = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = _testContentKey
            };
            
            //act
            var text = await service.GetSourceForKey(_testJavaScriptKey, game);
            
            //assert
            Assert.Equal(_testEnglishText, text);
        }

        [Fact]
        public async void GetSourceForKey_InvalidJavaScript_ReturnErrorText()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var text = await service.GetSourceForKey(_testBadJavaScriptKey);
            
            //assert
            Assert.Equal($"invalid JavaScript {_testBadJavaScriptKey}", text);
        }
        
        [Fact]
        public async void GetSourceForKey_InvalidJavaScriptReturn_ReturnErrorText()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var text = await service.GetSourceForKey(_testBadJavaScriptReturnKey);
            
            //assert
            Assert.Equal($"invalid JavaScript result {_testBadJavaScriptReturnKey}: banana", text);
        }
        
        #endregion
    }
}