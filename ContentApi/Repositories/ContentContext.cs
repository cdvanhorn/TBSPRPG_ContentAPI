using Microsoft.EntityFrameworkCore;
using TbspRpgLib.Repositories;

namespace ContentApi.Repositories {
    public class ContentContext : ServiceTrackingContext {
        public ContentContext(DbContextOptions<ContentContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");
        }
    }
}