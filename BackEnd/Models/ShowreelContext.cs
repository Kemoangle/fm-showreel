using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Showreel.Models;

public partial class ShowreelContext : DbContext
{
    public ShowreelContext()
    {
    }

    public ShowreelContext(DbContextOptions<ShowreelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<BuildingRestriction> BuildingRestrictions { get; set; }

    public virtual DbSet<Buildingplaylist> Buildingplaylists { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Landlordad> Landlordads { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Playlistvideo> Playlistvideos { get; set; }

    public virtual DbSet<RestrictionExcept> RestrictionExcepts { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<VideoType> VideoTypes { get; set; }

    public virtual DbSet<VideoVideolist> VideoVideolists { get; set; }

    public virtual DbSet<Videocategory> Videocategories { get; set; }

    public virtual DbSet<Videolist> Videolists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=sql12.freesqldatabase.com;uid=sql12666589;pwd=PcesrXAsfc;database=sql12666589", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.5.62-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("building")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.BuildingName)
                .HasMaxLength(255)
                .HasColumnName("building_name");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
            entity.Property(e => e.District)
                .HasMaxLength(255)
                .HasColumnName("district");
            entity.Property(e => e.LastUpdateTime).HasColumnName("last_update_time");
            entity.Property(e => e.PostalCode)
                .HasColumnType("int(11)")
                .HasColumnName("postal_code");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .HasColumnName("remark");
            entity.Property(e => e.Zone)
                .HasDefaultValueSql("'city'")
                .HasColumnType("enum('city','west','south','central','east','north')")
                .HasColumnName("zone");
        });

        modelBuilder.Entity<BuildingRestriction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("building_restriction");

            entity.HasIndex(e => e.CategoryId, "FK_BuildingRestriction_Category");

            entity.HasIndex(e => e.BuildingId, "building_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.BuildingId)
                .HasColumnType("int(11)")
                .HasColumnName("building_id");
            entity.Property(e => e.CategoryId)
                .HasColumnType("int(11)")
                .HasColumnName("categoryId");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasColumnType("enum('exclude','except')")
                .HasColumnName("type");

            entity.HasOne(d => d.Building).WithMany(p => p.BuildingRestrictions)
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("building_restriction_ibfk_1");

            entity.HasOne(d => d.Category).WithMany(p => p.BuildingRestrictions)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_BuildingRestriction_Category");
        });

        modelBuilder.Entity<Buildingplaylist>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("buildingplaylist")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.BuildingId, "building_id");

            entity.HasIndex(e => e.PlaylistId, "playlist_id");

            entity.Property(e => e.BuildingId)
                .HasColumnType("int(11)")
                .HasColumnName("building_id");
            entity.Property(e => e.PlaylistId)
                .HasColumnType("int(11)")
                .HasColumnName("playlist_id");

            entity.HasOne(d => d.Building).WithMany()
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("buildingplaylist_ibfk_1");

            entity.HasOne(d => d.Playlist).WithMany()
                .HasForeignKey(d => d.PlaylistId)
                .HasConstraintName("buildingplaylist_ibfk_2");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("category")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Landlordad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("landlordads")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.BuildingId, "fk_landlordads_building");

            entity.HasIndex(e => e.VideoId, "video_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.BuildingId)
                .HasColumnType("int(11)")
                .HasColumnName("building_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Loop)
                .HasColumnType("int(11)")
                .HasColumnName("loop");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.VideoId)
                .HasColumnType("int(11)")
                .HasColumnName("video_id");

            entity.HasOne(d => d.Building).WithMany(p => p.Landlordads)
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("fk_landlordads_building");

            entity.HasOne(d => d.Video).WithMany(p => p.Landlordads)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("landlordads_ibfk_1");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("playlist")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Creator)
                .HasMaxLength(255)
                .HasColumnName("creator");
            entity.Property(e => e.Duration)
                .HasMaxLength(255)
                .HasColumnName("duration");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'pending'")
                .HasColumnType("enum('pending','active','expired')")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Playlistvideo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("playlistvideo")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.PlaylistId, "playlist_id");

            entity.HasIndex(e => e.VideoId, "video_id");

            entity.Property(e => e.PlaylistId)
                .HasColumnType("int(11)")
                .HasColumnName("playlist_id");
            entity.Property(e => e.VideoId)
                .HasColumnType("int(11)")
                .HasColumnName("video_id");

            entity.HasOne(d => d.Playlist).WithMany()
                .HasForeignKey(d => d.PlaylistId)
                .HasConstraintName("playlistvideo_ibfk_2");

            entity.HasOne(d => d.Video).WithMany()
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("playlistvideo_ibfk_1");
        });

        modelBuilder.Entity<RestrictionExcept>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("restriction_except");

            entity.HasIndex(e => e.BuildingRestrictionId, "building_restriction_id");

            entity.HasIndex(e => e.VideoTypeId, "video_type_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.BuildingRestrictionId)
                .HasColumnType("int(11)")
                .HasColumnName("building_restriction_id");
            entity.Property(e => e.VideoTypeId)
                .HasColumnType("int(11)")
                .HasColumnName("video_type_id");

            entity.HasOne(d => d.BuildingRestriction).WithMany(p => p.RestrictionExcepts)
                .HasForeignKey(d => d.BuildingRestrictionId)
                .HasConstraintName("restriction_except_ibfk_1");

            entity.HasOne(d => d.VideoType).WithMany(p => p.RestrictionExcepts)
                .HasForeignKey(d => d.VideoTypeId)
                .HasConstraintName("restriction_except_ibfk_2");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("video")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.VideoTypeId, "fk_video_video_type");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
            entity.Property(e => e.Duration)
                .HasColumnType("int(11)")
                .HasColumnName("duration");
            entity.Property(e => e.FilePath)
                .HasMaxLength(255)
                .HasColumnName("file_path");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.KeyNo)
                .HasMaxLength(45)
                .HasColumnName("key_no");
            entity.Property(e => e.LastUpdateTime).HasColumnName("last_update_time");
            entity.Property(e => e.Rule)
                .HasMaxLength(255)
                .HasColumnName("rule");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.VideoTypeId)
                .HasColumnType("int(11)")
                .HasColumnName("video_type_id");

            entity.HasOne(d => d.VideoType).WithMany(p => p.Videos)
                .HasForeignKey(d => d.VideoTypeId)
                .HasConstraintName("fk_video_video_type");
        });

        modelBuilder.Entity<VideoType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("video_type");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<VideoVideolist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("video_videolist")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.VideoListId, "videoList_id");

            entity.HasIndex(e => e.VideoId, "video_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.LoopNum)
                .HasColumnType("int(11)")
                .HasColumnName("loop_num");
            entity.Property(e => e.VideoId)
                .HasColumnType("int(11)")
                .HasColumnName("video_id");
            entity.Property(e => e.VideoListId)
                .HasColumnType("int(11)")
                .HasColumnName("videoList_id");

            entity.HasOne(d => d.Video).WithMany(p => p.VideoVideolists)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("video_videolist_ibfk_1");

            entity.HasOne(d => d.VideoList).WithMany(p => p.VideoVideolists)
                .HasForeignKey(d => d.VideoListId)
                .HasConstraintName("video_videolist_ibfk_2");
        });

        modelBuilder.Entity<Videocategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("videocategory")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.HasIndex(e => e.CategoryId, "fk_category");

            entity.HasIndex(e => e.VideoId, "fk_video");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CategoryId)
                .HasColumnType("int(11)")
                .HasColumnName("category_id");
            entity.Property(e => e.VideoId)
                .HasColumnType("int(11)")
                .HasColumnName("video_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Videocategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("fk_category");

            entity.HasOne(d => d.Video).WithMany(p => p.Videocategories)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("fk_video");
        });

        modelBuilder.Entity<Videolist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("videolist")
                .HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedTime).HasColumnName("createdTime");
            entity.Property(e => e.LastUpdatedTime).HasColumnName("lastUpdatedTime");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .HasColumnName("remark");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
