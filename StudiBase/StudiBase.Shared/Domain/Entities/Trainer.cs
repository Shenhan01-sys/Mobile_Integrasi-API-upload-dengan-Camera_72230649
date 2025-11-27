using System.ComponentModel.DataAnnotations;

namespace StudiBase.Shared.Domain.Entities
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = default!;

        [Required, MaxLength(200), EmailAddress]
        public string Email { get; set; } = default!;

        public string? Bio { get; set; }

        [MaxLength(30)]
        public string? Phone { get; set; }

        [MaxLength(500)]
        public string? PhotoUrl { get; set; }

        [MaxLength(500)]
        public string? LinkedInUrl { get; set; }

        [MaxLength(500)]
        public string? Skills { get; set; } // CSV sementara

        public int? YearsOfExperience { get; set; }

        public double RatingAverage { get; set; }

        public bool IsActive { get; set; } = true;
        public string? ProfilePicturePath { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }
    }
}
