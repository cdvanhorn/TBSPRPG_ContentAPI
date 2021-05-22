using System;
using ContentApi.Entities;
using ContentApi.Repositories;
using Xunit;

namespace ContentApi.Tests.Repositories
{
    public class ConditionalSourceRepositoryTests : InMemoryTest
    {
        #region Setup

        private readonly Guid _testContentKey = Guid.NewGuid();
        private const string _testJavaScript = "if(true) { return 'fb8f8b1b-d2bd-4a86-85d1-40757b03a15c'; }";

        public ConditionalSourceRepositoryTests() : base("ConditionalSourceRepositoryTests")
        {
            Seed();
        }

        private void Seed()
        {
            using var context = new ContentContext(_dbContextOptions);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var conditionalSource = new ConditionalSource()
            {
                Id = Guid.NewGuid(),
                ContentKey = _testContentKey,
                JavaScript = _testJavaScript
            };

            context.ConditionalSources.Add(conditionalSource);
            context.SaveChanges();
        }
        
        #endregion

        #region GetJavaScriptForKey

        [Fact]
        public async void GetJavaScriptForKey_ValidKey_ReturnsJavaScript()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var repository = new ConditionalSourceRepository(context);
            
            //act
            var js = await repository.GetJavaScriptForKey(_testContentKey);
            
            //assert
            Assert.NotNull(js);
            Assert.Equal(_testJavaScript, js);
        }
        
        [Fact]
        public async void GetJavaScriptForKey_InValidKey_ReturnsNull()
        {
            //arrange
            await using var context = new ContentContext(_dbContextOptions);
            var repository = new ConditionalSourceRepository(context);
            
            //act
            var js = await repository.GetJavaScriptForKey(Guid.NewGuid());
            
            //assert
            Assert.Null(js);
        }

        #endregion
    }
}