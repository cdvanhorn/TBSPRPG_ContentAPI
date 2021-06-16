using ContentApi.Entities;
using ContentApi.Entities.LanguageSources;
using Microsoft.EntityFrameworkCore;
using TbspRpgLib.Repositories;

namespace ContentApi.Repositories {
    public class ContentContext : ServiceTrackingContext {
        public ContentContext(DbContextOptions<ContentContext> options) : base(options){}
        
        public DbSet<Content> Contents { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<En> SourcesEn { get; set; }
        public DbSet<Esp> SourcesEsp { get; set; }
        public DbSet<ConditionalSource> ConditionalSources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");
            
            modelBuilder.Entity<Content>().ToTable("contents");
            modelBuilder.Entity<Game>().ToTable("games");
            modelBuilder.Entity<En>().ToTable("sources_en");
            modelBuilder.Entity<Esp>().ToTable("sources_esp");
            modelBuilder.Entity<ConditionalSource>().ToTable("conditional_sources");
            
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
            
            modelBuilder.Entity<ConditionalSource>().HasKey(c => c.Id);
            modelBuilder.Entity<ConditionalSource>().Property(c => c.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            //language sources
            modelBuilder.Entity<En>().HasKey(e => e.Id);
            modelBuilder.Entity<En>().Property(e => e.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();
            
            modelBuilder.Entity<Esp>().HasKey(e => e.Id);
            modelBuilder.Entity<Esp>().Property(e => e.Id)
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();
        }
    }
}