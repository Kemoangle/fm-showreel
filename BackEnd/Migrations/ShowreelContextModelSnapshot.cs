﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Showreel.Models;

#nullable disable

namespace Showreel.Migrations
{
    [DbContext(typeof(ShowreelContext))]
    partial class ShowreelContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb3_general_ci")
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");

            modelBuilder.Entity("Showreel.Models.Building", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("address");

                    b.Property<string>("BuildingName")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("building_name");

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("create_time");

                    b.Property<string>("District")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("district");

                    b.Property<DateTime?>("LastUpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("last_update_time");

                    b.Property<int?>("PostalCode")
                        .HasColumnType("int")
                        .HasColumnName("postal_code");

                    b.Property<string>("Remark")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("remark");

                    b.Property<string>("Zone")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("enum('city','west','south','central','east','north')")
                        .HasColumnName("zone")
                        .HasDefaultValueSql("'city'");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("building", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("Showreel.Models.Buildingplaylist", b =>
                {
                    b.Property<int?>("BuildingId")
                        .HasColumnType("int")
                        .HasColumnName("building_id");

                    b.Property<int?>("PlaylistId")
                        .HasColumnType("int")
                        .HasColumnName("playlist_id");

                    b.HasIndex(new[] { "BuildingId" }, "building_id");

                    b.HasIndex(new[] { "PlaylistId" }, "playlist_id");

                    b.ToTable("buildingplaylist", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("Showreel.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("name");

                    b.Property<int?>("Parent")
                        .HasColumnType("int")
                        .HasColumnName("parent");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("category", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("Showreel.Models.Landlordad", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int?>("BuildingId")
                        .HasColumnType("int")
                        .HasColumnName("building_id");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("end_date");

                    b.Property<int?>("Loop")
                        .HasColumnType("int")
                        .HasColumnName("loop");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("start_date");

                    b.Property<int?>("VideoId")
                        .HasColumnType("int")
                        .HasColumnName("video_id");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "BuildingId" }, "fk_landlordads_building");

                    b.HasIndex(new[] { "VideoId" }, "video_id");

                    b.ToTable("landlordads", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("Showreel.Models.Playlist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Creator")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("creator");

                    b.Property<string>("Duration")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("duration");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("end_date");

                    b.Property<string>("JsonPlaylist")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("start_date");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("enum('pending','active','expired')")
                        .HasColumnName("status")
                        .HasDefaultValueSql("'pending'");

                    b.Property<string>("Title")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("playlist", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("Showreel.Models.Playlistvideo", b =>
                {
                    b.Property<int?>("PlaylistId")
                        .HasColumnType("int")
                        .HasColumnName("playlist_id");

                    b.Property<int?>("VideoId")
                        .HasColumnType("int")
                        .HasColumnName("video_id");

                    b.HasIndex(new[] { "PlaylistId" }, "playlist_id");

                    b.HasIndex(new[] { "VideoId" }, "video_id");

                    b.ToTable("playlistvideo", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("Showreel.Models.Restriction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("ArrCategory")
                        .HasColumnType("json")
                        .HasColumnName("arr_category");

                    b.Property<int?>("BuildingId")
                        .HasColumnType("int")
                        .HasColumnName("building_id");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("category_id");

                    b.Property<string>("Type")
                        .HasColumnType("enum('Exclude','Except')")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "BuildingId" }, "building_id");

                    b.HasIndex(new[] { "CategoryId" }, "category_id");

                    b.ToTable("restriction", (string)null);
                });

            modelBuilder.Entity("Showreel.Models.Rule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int?>("DoNotPlay")
                        .HasColumnType("int")
                        .HasColumnName("do_not_play");

                    b.Property<int?>("NoBackToBack")
                        .HasColumnType("int")
                        .HasColumnName("no_back_to_back");

                    b.Property<int?>("VideoId")
                        .HasColumnType("int")
                        .HasColumnName("video_id");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "VideoId" }, "rule_ibfk_1");

                    b.HasIndex(new[] { "NoBackToBack" }, "rule_ibfk_2");

                    b.HasIndex(new[] { "DoNotPlay" }, "rule_ibfk_3");

                    b.ToTable("rule", (string)null);

                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb4_unicode_ci");
                });

            modelBuilder.Entity("Showreel.Models.Video", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime?>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("create_time");

                    b.Property<int?>("Duration")
                        .HasColumnType("int")
                        .HasColumnName("duration");

                    b.Property<string>("FilePath")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("file_path");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_active");

                    b.Property<string>("KeyNo")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("key_no");

                    b.Property<DateTime?>("LastUpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("last_update_time");

                    b.Property<string>("Remark")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("remark");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("video", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("Showreel.Models.VideoVideolist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int?>("LoopNum")
                        .HasColumnType("int")
                        .HasColumnName("loop_num");

                    b.Property<int?>("VideoId")
                        .HasColumnType("int")
                        .HasColumnName("video_id");

                    b.Property<int?>("VideoListId")
                        .HasColumnType("int")
                        .HasColumnName("videoList_id");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "VideoListId" }, "videoList_id");

                    b.HasIndex(new[] { "VideoId" }, "video_id")
                        .HasDatabaseName("video_id1");

                    b.ToTable("video_videolist", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("Showreel.Models.Videocategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("category_id");

                    b.Property<int?>("VideoId")
                        .HasColumnType("int")
                        .HasColumnName("video_id");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "CategoryId" }, "fk_category");

                    b.HasIndex(new[] { "VideoId" }, "fk_video");

                    b.ToTable("videocategory", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("Showreel.Models.Videolist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime?>("CreatedTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("createdTime");

                    b.Property<DateTime?>("LastUpdatedTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("lastUpdatedTime");

                    b.Property<string>("Remark")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("remark");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("videolist", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("Showreel.Models.Buildingplaylist", b =>
                {
                    b.HasOne("Showreel.Models.Building", "Building")
                        .WithMany()
                        .HasForeignKey("BuildingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("buildingplaylist_ibfk_1");

                    b.HasOne("Showreel.Models.Playlist", "Playlist")
                        .WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("buildingplaylist_ibfk_2");

                    b.Navigation("Building");

                    b.Navigation("Playlist");
                });

            modelBuilder.Entity("Showreel.Models.Landlordad", b =>
                {
                    b.HasOne("Showreel.Models.Building", "Building")
                        .WithMany("Landlordads")
                        .HasForeignKey("BuildingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_landlordads_building");

                    b.HasOne("Showreel.Models.Video", "Video")
                        .WithMany("Landlordads")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("landlordads_ibfk_1");

                    b.Navigation("Building");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Showreel.Models.Playlistvideo", b =>
                {
                    b.HasOne("Showreel.Models.Playlist", "Playlist")
                        .WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("playlistvideo_ibfk_2");

                    b.HasOne("Showreel.Models.Video", "Video")
                        .WithMany()
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("playlistvideo_ibfk_1");

                    b.Navigation("Playlist");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Showreel.Models.Restriction", b =>
                {
                    b.HasOne("Showreel.Models.Building", "Building")
                        .WithMany("Restrictions")
                        .HasForeignKey("BuildingId")
                        .HasConstraintName("restriction_ibfk_2");

                    b.HasOne("Showreel.Models.Category", "Category")
                        .WithMany("Restrictions")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("restriction_ibfk_1");

                    b.Navigation("Building");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Showreel.Models.Rule", b =>
                {
                    b.HasOne("Showreel.Models.Building", "DoNotPlayNavigation")
                        .WithMany("Rules")
                        .HasForeignKey("DoNotPlay")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("rule_ibfk_3");

                    b.HasOne("Showreel.Models.Category", "NoBackToBackNavigation")
                        .WithMany("Rules")
                        .HasForeignKey("NoBackToBack")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("rule_ibfk_2");

                    b.HasOne("Showreel.Models.Video", "Video")
                        .WithMany("Rules")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("rule_ibfk_1");

                    b.Navigation("DoNotPlayNavigation");

                    b.Navigation("NoBackToBackNavigation");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Showreel.Models.VideoVideolist", b =>
                {
                    b.HasOne("Showreel.Models.Video", "Video")
                        .WithMany("VideoVideolists")
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("video_videolist_ibfk_1");

                    b.HasOne("Showreel.Models.Videolist", "VideoList")
                        .WithMany("VideoVideolists")
                        .HasForeignKey("VideoListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("video_videolist_ibfk_2");

                    b.Navigation("Video");

                    b.Navigation("VideoList");
                });

            modelBuilder.Entity("Showreel.Models.Videocategory", b =>
                {
                    b.HasOne("Showreel.Models.Category", "Category")
                        .WithMany("Videocategories")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("fk_category");

                    b.HasOne("Showreel.Models.Video", "Video")
                        .WithMany("Videocategories")
                        .HasForeignKey("VideoId")
                        .HasConstraintName("fk_video");

                    b.Navigation("Category");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Showreel.Models.Building", b =>
                {
                    b.Navigation("Landlordads");

                    b.Navigation("Restrictions");

                    b.Navigation("Rules");
                });

            modelBuilder.Entity("Showreel.Models.Category", b =>
                {
                    b.Navigation("Restrictions");

                    b.Navigation("Rules");

                    b.Navigation("Videocategories");
                });

            modelBuilder.Entity("Showreel.Models.Video", b =>
                {
                    b.Navigation("Landlordads");

                    b.Navigation("Rules");

                    b.Navigation("VideoVideolists");

                    b.Navigation("Videocategories");
                });

            modelBuilder.Entity("Showreel.Models.Videolist", b =>
                {
                    b.Navigation("VideoVideolists");
                });
#pragma warning restore 612, 618
        }
    }
}
