using FileSharingPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace FileSharingPlatform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<FFile> Files { get; set; }
        public DbSet<PublicShareLink> PublicShareLinks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================
            // USER CONFIGURATION
            // ============================
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PasswordHash).IsRequired();
            });

            // ============================
            // FOLDER CONFIGURATION
            // ============================
            modelBuilder.Entity<Folder>(entity =>
            {
                entity.HasKey(e => e.FolderId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Path).IsRequired().HasMaxLength(2000);

                // Self-referencing
                entity.HasOne(f => f.ParentFolder)
                    .WithMany(f => f.SubFolders)
                    .HasForeignKey(f => f.ParentFolderId)
                    .OnDelete(DeleteBehavior.Restrict);

                // User → Folder
                entity.HasOne(f => f.User)
                    .WithMany(u => u.Folders)
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Unique folder name within same parent
                entity.HasIndex(e => new { e.Name, e.ParentFolderId, e.UserId })
                    .IsUnique();
            });

            // ============================
            // FILE CONFIGURATION
            // ============================
            modelBuilder.Entity<FFile>(entity =>
            {
                entity.HasKey(e => e.FileId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.StoragePath).IsRequired().HasMaxLength(500);
                entity.Property(e => e.MimeType).IsRequired().HasMaxLength(100);

                // Folder → File
                entity.HasOne(f => f.Folder)
                    .WithMany(fo => fo.Files)
                    .HasForeignKey(f => f.FolderId)
                    .OnDelete(DeleteBehavior.Restrict);

                // User → File
                entity.HasOne(f => f.User)
                    .WithMany(u => u.Files)
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Unique file name inside a folder
                entity.HasIndex(e => new { e.Name, e.FolderId })
                    .IsUnique();
            });

            // ============================
            // PUBLIC SHARE LINK CONFIGURATION
            // ============================
            modelBuilder.Entity<PublicShareLink>(entity =>
            {
                entity.HasKey(e => e.ShareLinkId);

                entity.HasIndex(e => e.Token).IsUnique();
                entity.Property(e => e.Token).IsRequired().HasMaxLength(32);

                // User → ShareLinks
                entity.HasOne(s => s.User)
                    .WithMany(u => u.ShareLinks)
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                // File → ShareLinks
                entity.HasOne(s => s.File)
                    .WithMany(f => f.ShareLinks)
                    .HasForeignKey(s => s.FileId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Folder → ShareLinks
                entity.HasOne(s => s.Folder)
                    .WithMany(f => f.ShareLinks)
                    .HasForeignKey(s => s.FolderId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
