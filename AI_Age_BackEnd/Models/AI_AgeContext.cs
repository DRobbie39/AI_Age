using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AI_Age_BackEnd.Models;

public partial class AI_AgeContext : DbContext
{
    public AI_AgeContext()
    {
    }

    public AI_AgeContext(DbContextOptions<AI_AgeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aitool> Aitools { get; set; }

    public virtual DbSet<AitoolCategory> AitoolCategories { get; set; }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<ArticleCategory> ArticleCategories { get; set; }

    public virtual DbSet<ArticleComment> ArticleComments { get; set; }

    public virtual DbSet<ArticleRating> ArticleRatings { get; set; }

    public virtual DbSet<ChatHistory> ChatHistories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPost> UserPosts { get; set; }

    public virtual DbSet<UserPostCategory> UserPostCategories { get; set; }

    public virtual DbSet<UserPostComment> UserPostComments { get; set; }

    public virtual DbSet<VideoArticle> VideoArticles { get; set; }

    public virtual DbSet<VideoArticleCategory> VideoArticleCategories { get; set; }

    public virtual DbSet<VideoArticleComment> VideoArticleComments { get; set; }

    public virtual DbSet<VideoArticleRating> VideoArticleRatings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-UTPMHK27\\SQLEXPRESS;Database=AI_Age;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aitool>(entity =>
        {
            entity.HasKey(e => e.ToolId).HasName("PK__AITools__CC0CEBB18FABEBB4");

            entity.ToTable("AITools");

            entity.Property(e => e.ToolId).HasColumnName("ToolID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.LogoUrl)
                .HasMaxLength(255)
                .HasColumnName("LogoURL");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.ToolName).HasMaxLength(100);
            entity.Property(e => e.WebsiteUrl)
                .HasMaxLength(255)
                .HasColumnName("WebsiteURL");

            entity.HasOne(d => d.Category).WithMany(p => p.Aitools)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__AITools__Categor__59063A47");
        });

        modelBuilder.Entity<AitoolCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__AIToolCa__19093A2B5E07392D");

            entity.ToTable("AIToolCategories");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.ArticleId).HasName("PK__Articles__9C6270C82BF31F6A");

            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.AverageRating)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(3, 1)");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Content).HasColumnType("ntext");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Level).HasDefaultValue(1);
            entity.Property(e => e.PostedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Summary).HasMaxLength(500);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.ToolId).HasColumnName("ToolID");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Views).HasDefaultValue(0);

            entity.HasOne(d => d.AuthorNavigation).WithMany(p => p.Articles)
                .HasForeignKey(d => d.Author)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Articles__Author__6477ECF3");

            entity.HasOne(d => d.Category).WithMany(p => p.Articles)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Articles__Catego__6383C8BA");

            entity.HasOne(d => d.Tool).WithMany(p => p.Articles)
                .HasForeignKey(d => d.ToolId)
                .HasConstraintName("FK__Articles__ToolID__656C112C");
        });

        modelBuilder.Entity<ArticleCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__ArticleC__19093A2BBF926BF6");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<ArticleComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__ArticleC__C3B4DFAAAB8FF6B7");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.CommentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Content).HasMaxLength(500);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleComments)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArticleCo__Artic__0A9D95DB");

            entity.HasOne(d => d.User).WithMany(p => p.ArticleComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArticleCo__UserI__0B91BA14");
        });

        modelBuilder.Entity<ArticleRating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__ArticleR__FCCDF85C7D75F579");

            entity.Property(e => e.RatingId).HasColumnName("RatingID");
            entity.Property(e => e.ArticleId).HasColumnName("ArticleID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Article).WithMany(p => p.ArticleRatings)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ArticleRa__Artic__6A30C649");

            entity.HasOne(d => d.User).WithMany(p => p.ArticleRatings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ArticleRa__UserI__6B24EA82");
        });

        modelBuilder.Entity<ChatHistory>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("PK__ChatHist__A9FBE6269CEB044D");

            entity.ToTable("ChatHistory");

            entity.Property(e => e.ChatId).HasColumnName("ChatID");
            entity.Property(e => e.ChatDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Question).HasMaxLength(500);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.ChatHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChatHisto__UserI__18EBB532");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3AAD6EAFC1");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.RoleName).HasMaxLength(50);
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC0B68A92D");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4F48796EF").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleID__5070F446");
        });

        modelBuilder.Entity<UserPost>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__UserPost__AA1260382244ACB3");

            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.AttachedImage).HasMaxLength(255);
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Content).HasColumnType("ntext");
            entity.Property(e => e.PostedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Views).HasDefaultValue(0);

            entity.HasOne(d => d.Category).WithMany(p => p.UserPosts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserPosts__Categ__05D8E0BE");

            entity.HasOne(d => d.User).WithMany(p => p.UserPosts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserPosts__UserI__06CD04F7");
        });

        modelBuilder.Entity<UserPostCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__UserPost__19093A2B36D27959");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<UserPostComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__UserPost__C3B4DFAABCC78A07");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.CommentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Content).HasMaxLength(500);
            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Post).WithMany(p => p.UserPostComments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserPostC__PostI__14270015");

            entity.HasOne(d => d.User).WithMany(p => p.UserPostComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserPostC__UserI__151B244E");
        });

        modelBuilder.Entity<VideoArticle>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("PK__VideoArt__BAE5124AEB4F29CC");

            entity.Property(e => e.VideoId).HasColumnName("VideoID");
            entity.Property(e => e.AverageRating)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(3, 1)");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Level).HasDefaultValue(1);
            entity.Property(e => e.PostedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Thumbnail).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.ToolId).HasColumnName("ToolID");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.VideoUrl)
                .HasMaxLength(255)
                .HasColumnName("VideoURL");
            entity.Property(e => e.Views).HasDefaultValue(0);

            entity.HasOne(d => d.Category).WithMany(p => p.VideoArticles)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoArti__Categ__75A278F5");

            entity.HasOne(d => d.Tool).WithMany(p => p.VideoArticles)
                .HasForeignKey(d => d.ToolId)
                .HasConstraintName("FK__VideoArti__ToolI__778AC167");

            entity.HasOne(d => d.UploaderNavigation).WithMany(p => p.VideoArticles)
                .HasForeignKey(d => d.Uploader)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoArti__Uploa__76969D2E");
        });

        modelBuilder.Entity<VideoArticleCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__VideoArt__19093A2B153C05F8");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<VideoArticleComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__VideoArt__C3B4DFAAAA002D37");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.CommentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Content).HasMaxLength(500);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");

            entity.HasOne(d => d.User).WithMany(p => p.VideoArticleComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoArti__UserI__10566F31");

            entity.HasOne(d => d.Video).WithMany(p => p.VideoArticleComments)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoArti__Video__0F624AF8");
        });

        modelBuilder.Entity<VideoArticleRating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__VideoArt__FCCDF85CD6F5F3F6");

            entity.Property(e => e.RatingId).HasColumnName("RatingID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.VideoArticleRatings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__VideoArti__UserI__7D439ABD");

            entity.HasOne(d => d.Video).WithMany(p => p.VideoArticleRatings)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoArti__Video__7C4F7684");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
