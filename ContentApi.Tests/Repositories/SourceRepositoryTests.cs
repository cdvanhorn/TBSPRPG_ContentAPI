using ContentApi.Repositories;
using Xunit;

namespace ContentApi.Tests.Repositories
{
    public class SourceRepositoryTests : InMemoryTest
    {
        #region Setup

        public SourceRepositoryTests() : base("SourceRepositoryTests")
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

        #endregion

        #region GetSourceForKey

        [Fact]
        public async void GetSourceForKey_Valid_SourceReturned()
        {
            
        }

        [Fact]
        public async void GetSourceForKey_InvalidKey_ReturnNone()
        {
            
        }

        [Fact]
        public async void GetSourceForKey_ChangeLanguage_SourceReturnedInLanguage()
        {
            
        }

        #endregion
    }
}