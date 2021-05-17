using System;
using System.Threading.Tasks;
using ContentApi.Entities.LanguageSources;
using ContentApi.Repositories;
using ContentApi.Services;
using Xunit;

namespace ContentApi.Tests.Services
{
    public class SourceServiceTests : InMemoryTest
    {
        #region Setup
        
        private readonly Guid _testContentKey = Guid.NewGuid();
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
            
            context.SourcesEn.AddRange(enSource, enSource2);
            context.SourcesEsp.Add(espSource);
            context.SaveChanges();
        }

        private static SourceService CreateService(ContentContext context)
        {
            return new SourceService(new SourceRepository(context));
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
        public async void GetSourceForKey_InvalidKey_ReturnNone()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var text = await service.GetSourceForKey(Guid.NewGuid());
            
            //assert
            Assert.Null(text);
        }

        [Fact]
        public async void GetSourceForKey_ChangeLanguage_SourceReturnedInLanguage()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var service = CreateService(context);
            
            //act
            var text = await service.GetSourceForKey(_testContentKey, SourceRepository.SPANISH);
            
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
            Task Act() => service.GetSourceForKey(_testContentKey, "banana");

            //assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        #endregion
    }
}