using System;
using System.ComponentModel.DataAnnotations;
using StudiBase.Shared.Domain.Enums;

namespace StudiBase.Shared.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        // Tambahan
        [MaxLength(100)]
        public string? Category { get; set; }

        [MaxLength(10)]
        public string? Language { get; set; }

        public int Duration { get; set; } // minutes
        public int TotalLessons { get; set; }
        public bool IsFree { get; set; }

        public CourseLevel Level { get; set; } = CourseLevel.Beginner;
        public CourseStatus Status { get; set; } = CourseStatus.Draft;
        public CourseVisibility Visibility { get; set; } = CourseVisibility.Public;

        // Pakai decimal untuk harga; SQLite akan memetakannya dengan baik
        public decimal Price { get; set; }

        public DateTime? PublishedOn { get; set; }
        public string? thumbnailImagePath { get; set; }

        // Relasi ke Trainer
        [Required]
        public int TrainerId { get; set; }

        public Trainer? Trainer { get; set; }
    }
}
