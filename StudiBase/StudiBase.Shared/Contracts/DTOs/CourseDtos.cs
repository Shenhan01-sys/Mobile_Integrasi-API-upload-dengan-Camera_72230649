using System;
using System.ComponentModel.DataAnnotations;

namespace StudiBase.Shared.Contracts.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Level { get; set; } // string representation of enum
        public string? Status { get; set; } // string representation of enum
        public string? Visibility { get; set; } // string representation of enum
        public string? Language { get; set; }
        public int Duration { get; set; }
        public int TotalLessons { get; set; }
        public bool IsFree { get; set; }
        public decimal Price { get; set; }
        public DateTime? PublishedOn { get; set; }
        public string? ThumbnailImagePath { get; set; }
        public int TrainerId { get; set; }
        public string? TrainerName { get; set; }
    }

    public class CourseCreateDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        [MaxLength(100)]
        public string? Category { get; set; }
        [MaxLength(20)]
        public string? Level { get; set; }
        [MaxLength(20)]
        public string? Status { get; set; }
        [MaxLength(20)]
        public string? Visibility { get; set; }
        [MaxLength(10)]
        public string? Language { get; set; }
        [Range(0, int.MaxValue)]
        public int Duration { get; set; }
        [Range(0, int.MaxValue)]
        public int TotalLessons { get; set; }
        public bool IsFree { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public DateTime? PublishedOn { get; set; }
        [Required]
        public int TrainerId { get; set; }
    }

    public class CourseUpdateDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        [MaxLength(100)]
        public string? Category { get; set; }
        [MaxLength(20)]
        public string? Level { get; set; }
        [MaxLength(20)]
        public string? Status { get; set; }
        [MaxLength(20)]
        public string? Visibility { get; set; }
        [MaxLength(10)]
        public string? Language { get; set; }
        [Range(0, int.MaxValue)]
        public int Duration { get; set; }
        [Range(0, int.MaxValue)]
        public int TotalLessons { get; set; }
        public bool IsFree { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public DateTime? PublishedOn { get; set; }
        [Required]
        public int TrainerId { get; set; }
    }
}
