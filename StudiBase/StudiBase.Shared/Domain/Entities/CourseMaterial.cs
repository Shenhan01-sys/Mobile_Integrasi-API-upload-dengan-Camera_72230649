using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudiBase.Shared.Domain.Enums;

namespace StudiBase.Shared.Domain.Entities
{
    public class CourseMaterial
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string FilePath { get; set; } = default!;
        public FileType FileTypeMaterial { get; set; } = FileType.pdf;
        public long FileSize { get; set; }
        public DateTime UploadedOn { get; set; }

        [Required]
        public int UploadedByTrainerId { get; set; }
        public Trainer? Trainer { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
