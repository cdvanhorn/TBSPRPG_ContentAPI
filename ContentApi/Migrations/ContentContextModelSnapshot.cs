﻿// <auto-generated />
using System;
using ContentApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ContentApi.Migrations
{
    [DbContext(typeof(ContentContext))]
    partial class ContentContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasPostgresExtension("uuid-ossp")
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("ContentApi.Entities.ConditionalSource", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("ContentKey")
                        .HasColumnType("uuid");

                    b.Property<string>("JavaScript")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("conditional_sources");
                });

            modelBuilder.Entity("ContentApi.Entities.Content", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Position")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("contents");
                });

            modelBuilder.Entity("ContentApi.Entities.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("AdventureId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("games");
                });

            modelBuilder.Entity("ContentApi.Entities.LanguageSources.En", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("ContentKey")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("sources_en");
                });

            modelBuilder.Entity("ContentApi.Entities.LanguageSources.Esp", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("ContentKey")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("sources_esp");
                });

            modelBuilder.Entity("TbspRpgLib.Entities.EventTypePosition", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("EventTypeId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Position")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.ToTable("event_type_positions");
                });

            modelBuilder.Entity("TbspRpgLib.Entities.ProcessedEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("processed_events");
                });
#pragma warning restore 612, 618
        }
    }
}
