using System.Collections.Generic;
using ContentApi.Repositories;
using ContentApi.Services;
using TbspRpgLib.Events;

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
    }
}