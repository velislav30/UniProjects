﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VT.Data;

namespace VT.Data.Migrations
{
    [DbContext(typeof(VTorrentsDbContext))]
    [Migration("20200613101409_ModifyDatabase")]
    partial class ModifyDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VT.Models.Entities.Catalog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastDownloadedFrom")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TorrentNum")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Catalogs");
                });

            modelBuilder.Entity("VT.Models.Entities.SubType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CatalogId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CatalogId");

                    b.HasIndex("CreatorId");

                    b.ToTable("SubTypes");
                });

            modelBuilder.Entity("VT.Models.Entities.Torrent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CatalogId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<int>("SubTypeId")
                        .HasColumnType("int");

                    b.Property<int>("TimesDownloaded")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UploadedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("UploaderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CatalogId");

                    b.HasIndex("SubTypeId");

                    b.HasIndex("UploaderId");

                    b.ToTable("Torrents");
                });

            modelBuilder.Entity("VT.Models.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastLoggedIn")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isAdmin")
                        .HasColumnType("bit");

                    b.Property<bool>("isMod")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("VT.Models.Entities.UserToTorrent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DeletedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("DownloaderId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("TorrentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DownloaderId");

                    b.HasIndex("TorrentId");

                    b.ToTable("UserToTorrents");
                });

            modelBuilder.Entity("VT.Models.Entities.Catalog", b =>
                {
                    b.HasOne("VT.Models.Entities.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VT.Models.Entities.SubType", b =>
                {
                    b.HasOne("VT.Models.Entities.Catalog", "Catalog")
                        .WithMany("UploadedTorrents")
                        .HasForeignKey("CatalogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VT.Models.Entities.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VT.Models.Entities.Torrent", b =>
                {
                    b.HasOne("VT.Models.Entities.Catalog", "Catalog")
                        .WithMany("Torrents")
                        .HasForeignKey("CatalogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VT.Models.Entities.SubType", "SybType")
                        .WithMany("UploadedTorrents")
                        .HasForeignKey("SubTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VT.Models.Entities.User", "Uploader")
                        .WithMany()
                        .HasForeignKey("UploaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VT.Models.Entities.UserToTorrent", b =>
                {
                    b.HasOne("VT.Models.Entities.User", "Downloader")
                        .WithMany()
                        .HasForeignKey("DownloaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VT.Models.Entities.Torrent", "Torrent")
                        .WithMany("Downloads")
                        .HasForeignKey("TorrentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
