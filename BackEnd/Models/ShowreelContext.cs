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

    public virtual DbSet<Buildingplaylist> Buildingplaylists { get; set; }

    public virtual DbSet<Buildingrestriction> Buildingrestrictions { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Landlordad> Landlordads { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Playlistvideo> Playlistvideos { get; set; }

    public virtual DbSet<Restriction> Restrictions { get; set; }

    public virtual DbSet<Subcategory> Subcategories { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<Videocategory> Videocategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=127.0.0.1;uid=root;pwd=26072001;database=Showreel", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Building>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("building");

            entity.Property(e => e.Id).HasColumnName("id");
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
            entity.Property(e => e.PostalCode).HasColumnName("postal_code");
            entity.Property(e => e.Remark)
                .HasMaxLength(255)
                .HasColumnName("remark");
            entity.Property(e => e.Zone)
                .HasDefaultValueSql("'city'")
                .HasColumnType("enum('city','west','south','central','east','north')")
                .HasColumnName("zone");
        });

        modelBuilder.Entity<Buildingplaylist>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("buildingplaylist");

            entity.HasIndex(e => e.BuildingId, "building_id");

            entity.HasIndex(e => e.PlaylistId, "playlist_id");

            entity.Property(e => e.BuildingId).HasColumnName("building_id");
            entity.Property(e => e.PlaylistId).HasColumnName("playlist_id");

            entity.HasOne(d => d.Building).WithMany()
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("buildingplaylist_ibfk_1");

            entity.HasOne(d => d.Playlist).WithMany()
                .HasForeignKey(d => d.PlaylistId)
                .HasConstraintName("buildingplaylist_ibfk_2");
        });

        modelBuilder.Entity<Buildingrestriction>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("buildingrestriction");

            entity.HasIndex(e => e.BuildingId, "building_id");

            entity.HasIndex(e => e.RestrictionId, "restriction_id");

            entity.Property(e => e.BuildingId).HasColumnName("building_id");
            entity.Property(e => e.Except)
                .HasColumnType("text")
                .HasColumnName("except");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.RestrictionId).HasColumnName("restriction_id");

            entity.HasOne(d => d.Building).WithMany()
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("buildingrestriction_ibfk_1");

            entity.HasOne(d => d.Restriction).WithMany()
                .HasForeignKey(d => d.RestrictionId)
                .HasConstraintName("buildingrestriction_ibfk_2");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Landlordad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("landlordads");

            entity.HasIndex(e => e.BuildingId, "building_id");

            entity.HasIndex(e => e.VideoId, "video_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BuildingId).HasColumnName("building_id");
            entity.Property(e => e.Loop).HasColumnName("loop");
            entity.Property(e => e.VideoId).HasColumnName("video_id");

            entity.HasOne(d => d.Building).WithMany(p => p.Landlordads)
                .HasForeignKey(d => d.BuildingId)
                .HasConstraintName("landlordads_ibfk_2");

            entity.HasOne(d => d.Video).WithMany(p => p.Landlordads)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("landlordads_ibfk_1");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("playlist");

            entity.Property(e => e.Id).HasColumnName("id");
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
                .ToTable("playlistvideo");

            entity.HasIndex(e => e.PlaylistId, "playlist_id");

            entity.HasIndex(e => e.VideoId, "video_id");

            entity.Property(e => e.PlaylistId).HasColumnName("playlist_id");
            entity.Property(e => e.VideoId).HasColumnName("video_id");

            entity.HasOne(d => d.Playlist).WithMany()
                .HasForeignKey(d => d.PlaylistId)
                .HasConstraintName("playlistvideo_ibfk_2");

            entity.HasOne(d => d.Video).WithMany()
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("playlistvideo_ibfk_1");
        });

        modelBuilder.Entity<Restriction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("restriction");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RestrictionName)
                .HasMaxLength(255)
                .HasColumnName("restriction_name");
        });

        modelBuilder.Entity<Subcategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("subcategory");

            entity.HasIndex(e => e.CategoryId, "category_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Category).WithMany(p => p.Subcategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("subcategory_ibfk_1");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("video");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateTime).HasColumnName("create_time");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.FilePath)
                .HasMaxLength(255)
                .HasColumnName("file_path");
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
        });

        modelBuilder.Entity<Videocategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("videocategory");

            entity.HasIndex(e => e.CategoryId, "fk_category");

            entity.HasIndex(e => e.VideoId, "fk_video");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.VideoId).HasColumnName("video_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Videocategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("fk_category");

            entity.HasOne(d => d.Video).WithMany(p => p.Videocategories)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("fk_video");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
