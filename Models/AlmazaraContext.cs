using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Back_Almazara.Models;

public partial class AlmazaraContext : DbContext
{
    public AlmazaraContext()
    {
    }

    public AlmazaraContext(DbContextOptions<AlmazaraContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TCategory> TCategories { get; set; }

    public virtual DbSet<TComment> TComments { get; set; }

    public virtual DbSet<TFavorite> TFavorites { get; set; }

    public virtual DbSet<TNotice> TNotices { get; set; }

    public virtual DbSet<TNoticeRelCategory> TNoticeRelCategories { get; set; }

    public virtual DbSet<TNoticesDetail> TNoticesDetails { get; set; }

    public virtual DbSet<TRole> TRoles { get; set; }

    public virtual DbSet<TUser> TUsers { get; set; }

    public virtual DbSet<TUsersRelRole> TUsersRelRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=192.168.1.15,1433;Database=Almazara;User Id=UsuarioAlmazara;Password=VqoWrEwQbBIF918S7m4EVB;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TCategory>(entity =>
        {
            entity.HasKey(e => e.IdI).HasName("PK__t_catego__9DB7D2FC6606B1A0");

            entity.ToTable("t_category");

            entity.Property(e => e.IdI).HasColumnName("id_i");
            entity.Property(e => e.ContentNv).HasColumnName("content_nv");
            entity.Property(e => e.DisableB).HasColumnName("disable_b");
            entity.Property(e => e.NoticeIdI).HasColumnName("notice_id_i");
            entity.Property(e => e.SubtitleNv)
                .HasMaxLength(100)
                .HasColumnName("subtitle_nv");
            entity.Property(e => e.TitleNv)
                .HasMaxLength(100)
                .HasColumnName("title_nv");
        });

        modelBuilder.Entity<TComment>(entity =>
        {
            entity.HasKey(e => e.IdI).HasName("PK__t_commen__9DB7D2FC08B1C771");

            entity.ToTable("t_comments");

            entity.Property(e => e.IdI).HasColumnName("id_i");
            entity.Property(e => e.ContentNv)
                .HasMaxLength(500)
                .HasColumnName("content_nv");
            entity.Property(e => e.DateDt)
                .HasColumnType("datetime")
                .HasColumnName("date_dt");
            entity.Property(e => e.DisableB).HasColumnName("disable_b");
            entity.Property(e => e.NoticeIdI).HasColumnName("notice_id_i");
            entity.Property(e => e.UserIdI).HasColumnName("user_id_i");
        });

        modelBuilder.Entity<TFavorite>(entity =>
        {
            entity.HasKey(e => e.IdI).HasName("PK__t_favori__9DB7D2FC5B748D0C");

            entity.ToTable("t_favorites");

            entity.Property(e => e.IdI).HasColumnName("id_i");
            entity.Property(e => e.DisableB).HasColumnName("disable_b");
            entity.Property(e => e.NoticeIdI).HasColumnName("notice_id_i");
            entity.Property(e => e.UserIdI).HasColumnName("user_id_i");
        });

        modelBuilder.Entity<TNotice>(entity =>
        {
            entity.HasKey(e => e.IdI).HasName("PK__t_notice__9DB7D2FCAD81C9DB");

            entity.ToTable("t_notices");

            entity.Property(e => e.IdI).HasColumnName("id_i");
            entity.Property(e => e.DescriptionNv)
                .HasColumnName("description_nv");
            entity.Property(e => e.DisableB).HasColumnName("disable_b");
            entity.Property(e => e.ImageNv).HasColumnName("image_nv");
            entity.Property(e => e.NameNv)
                .HasMaxLength(100)
                .HasColumnName("name_nv");
        });

        modelBuilder.Entity<TNoticeRelCategory>(entity =>
        {
            entity.HasKey(e => e.IdI).HasName("PK__t_notice__9DB7D2FC703FC7D0");

            entity.ToTable("t_notice_rel_category");

            entity.Property(e => e.IdI).HasColumnName("id_i");
            entity.Property(e => e.CategoryIdI).HasColumnName("category_id_i");
            entity.Property(e => e.DisableB).HasColumnName("disable_b");
            entity.Property(e => e.NoticeIdI).HasColumnName("notice_id_i");
        });

        modelBuilder.Entity<TNoticesDetail>(entity =>
        {
            entity.HasKey(e => e.IdI).HasName("PK__t_notice__9DB7D2FC28E88F02");

            entity.ToTable("t_notices_detail");

            entity.Property(e => e.IdI).HasColumnName("id_i");
            entity.Property(e => e.ContentNv).HasColumnName("content_nv");
            entity.Property(e => e.DisableB).HasColumnName("disable_b");
            entity.Property(e => e.NoticeIdI).HasColumnName("notice_id_i");
            entity.Property(e => e.SubtitleNv)
                .HasColumnName("subtitle_nv");
            entity.Property(e => e.TitleNv)
                .HasColumnName("title_nv");
        });

        modelBuilder.Entity<TRole>(entity =>
        {
            entity.HasKey(e => e.IdI).HasName("PK__t_roles__9DB7D2FCF9945A26");

            entity.ToTable("t_roles");

            entity.Property(e => e.IdI).HasColumnName("id_i");
            entity.Property(e => e.DisableB).HasColumnName("disable_b");
            entity.Property(e => e.NameNv)
                .HasMaxLength(100)
                .HasColumnName("name_nv");
            entity.Property(e => e.PermissionIdI).HasColumnName("permission_id_i");
        });

        modelBuilder.Entity<TUser>(entity =>
        {
            entity.HasKey(e => e.IdI).HasName("PK__t_users__9DB7D2FCAAD99D9E");

            entity.ToTable("t_users");

            entity.Property(e => e.IdI).HasColumnName("id_i");
            entity.Property(e => e.DisableB).HasColumnName("disable_b");
            entity.Property(e => e.EmailNv)
                .HasMaxLength(100)
                .HasColumnName("email_nv");
            entity.Property(e => e.NameNv)
                .HasMaxLength(100)
                .HasColumnName("name_nv");
            entity.Property(e => e.PasswordNv)
                .HasMaxLength(100)
                .HasColumnName("password_nv");
            entity.Property(e => e.RoleI)
                .HasDefaultValue(4)
                .HasColumnName("role_i");
        });

        modelBuilder.Entity<TUsersRelRole>(entity =>
        {
            entity.HasKey(e => e.IdI).HasName("PK__t_users___9DB7D2FCF2D211FE");

            entity.ToTable("t_users_rel_roles");

            entity.Property(e => e.IdI).HasColumnName("id_i");
            entity.Property(e => e.DisableB).HasColumnName("disable_b");
            entity.Property(e => e.RoleIdI).HasColumnName("role_id_i");
            entity.Property(e => e.UserIdI).HasColumnName("user_id_i");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
