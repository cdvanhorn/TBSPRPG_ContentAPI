using ContentApi.Entities;
using Microsoft.EntityFrameworkCore;
using TbspRpgLib.Repositories;

namespace ContentApi.Repositories {
    public class ContentContext : ServiceTrackingContext {
        public ContentContext(DbContextOptions<ContentContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");
            
            modelBuilder.Entity<Content>().ToTable("contents");
            modelBuilder.Entity<Game>().ToTable("games");
            
            modelBuilder.Entity<Game>().HasKey(g => g.Id);
            modelBuilder.Entity<Game>().Property(g => g.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();
            
            modelBuilder.Entity<Content>().HasKey(c => c.Id);
            modelBuilder.Entity<Content>().Property(c => c.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();
            
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Contents)
                .WithOne(c => c.Game)
                .HasForeignKey(c => c.GameId);
        }
    }
}