using System;
using System.ComponentModel.DataAnnotations;

namespace StudiBase.Shared.Contracts.DTOs
{
 public class CourseMaterialDto
 {
 public int Id { get; set; }
 public string Title { get; set; } = default!;
 public string? Description { get; set; }
 public string FilePath { get; set; } = default!; // relative path under wwwroot
 public string FileType { get; set; } = default!; // enum as string
 public long FileSize { get; set; }
 public DateTime UploadedOn { get; set; }
 public int UploadedByTrainerId { get; set; }
 public string? UploadedByTrainerName { get; set; }
 public int CourseId { get; set; }
 }

 // Metadata for creating a material; file is provided via multipart/form-data in controller
 public class CourseMaterialCreateDto
 {
 [Required, MaxLength(200)]
 public string Title { get; set; } = default!;
 [MaxLength(500)]
 public string? Description { get; set; }
 [Required]
 public int UploadedByTrainerId { get; set; }
 }
}
