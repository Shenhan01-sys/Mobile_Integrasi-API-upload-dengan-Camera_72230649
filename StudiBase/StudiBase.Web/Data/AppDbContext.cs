using Microsoft.EntityFrameworkCore;
using StudiBase.Shared.Domain.Entities;
using StudiBase.Shared.Domain.Enums;

namespace StudiBase.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Trainer> Trainers => Set<Trainer>();
        public DbSet<CourseMaterial> CourseMaterials => Set<CourseMaterial>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trainer>(b =>
            {
                b.Property(x => x.Name).IsRequired().HasMaxLength(200);
                b.Property(x => x.Email).IsRequired().HasMaxLength(200);
                b.Property(x => x.Phone).HasMaxLength(30);
                b.Property(x => x.PhotoUrl).HasMaxLength(500);
                b.Property(x => x.LinkedInUrl).HasMaxLength(500);
                b.Property(x => x.Skills).HasMaxLength(500);
                b.Property(x => x.Country).HasMaxLength(100);
                b.HasIndex(x => x.Name);
                b.Property(x => x.ProfilePicturePath).HasMaxLength(500);
            });

            modelBuilder.Entity<Course>(b =>
            {
                b.Property(x => x.Title).IsRequired().HasMaxLength(200);
                b.Property(x => x.Category).HasMaxLength(100);
                b.Property(x => x.Language).HasMaxLength(10);
                b.Property(x => x.Price).HasPrecision(18, 2);
                b.Property(x => x.Level).HasConversion<string>().HasMaxLength(20);
                b.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
                b.Property(x => x.Visibility).HasConversion<string>().HasMaxLength(20);
                b.HasIndex(x => x.Title);
                b.Property(x => x.thumbnailImagePath).HasMaxLength(500);
                b.HasOne(x => x.Trainer)
                 .WithMany()
                 .HasForeignKey(x => x.TrainerId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CourseMaterial>(b =>
            {
                b.Property(x => x.Title).IsRequired().HasMaxLength(200);
                b.Property(x => x.Description).HasMaxLength(500);
                b.Property(x => x.FilePath).IsRequired();
                b.Property(x => x.FileTypeMaterial).HasConversion<string>().HasMaxLength(10);
                b.Property(x => x.FileSize).IsRequired();
                b.Property(x => x.UploadedOn).IsRequired();

                b.HasOne(x => x.Course)
                 .WithMany()
                 .HasForeignKey(x => x.CourseId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(x => x.Trainer)
                 .WithMany()
                 .HasForeignKey(x => x.UploadedByTrainerId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Seed minimal (opsional)
            modelBuilder.Entity<Trainer>().HasData(new Trainer
            {
                Id = 1,
                Name = "Default Trainer",
                Email = "trainer@example.com",
                Bio = "Sample trainer",
                IsActive = true,
                RatingAverage = 0,
                ProfilePicturePath = ""
            });

            modelBuilder.Entity<Course>().HasData(new Course
            {
                Id = 1,
                Title = "Intro to Hybrid LMS",
                Description = "Sample course",
                Category = "General",
                Language = "en",
                Duration = 60,
                TotalLessons = 5,
                IsFree = true,
                Level = CourseLevel.Beginner,
                Status = CourseStatus.Published,
                Visibility = CourseVisibility.Public,
                Price = 0m,
                thumbnailImagePath = "",
                // gunakan nilai deterministik agar migrasi stabil
                PublishedOn = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                TrainerId = 1
            });
        }
    }
}
