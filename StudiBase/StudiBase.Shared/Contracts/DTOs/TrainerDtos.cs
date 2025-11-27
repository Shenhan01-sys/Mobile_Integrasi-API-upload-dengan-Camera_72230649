using System.ComponentModel.DataAnnotations;

namespace StudiBase.Shared.Contracts.DTOs
{
    public class TrainerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Bio { get; set; }
        public string? Phone { get; set; }
        public string? PhotoUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? Skills { get; set; }
        public int? YearsOfExperience { get; set; }
        public double RatingAverage { get; set; }
        public bool IsActive { get; set; }
        public string? Country { get; set; }
        public string? ProfilePicturePath { get; set; }
    }

    public class TrainerCreateDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = default!;
        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = default!;
        public string? Bio { get; set; }
        [MaxLength(30)]
        public string? Phone { get; set; }
        [Url, MaxLength(500)]
        public string? PhotoUrl { get; set; }
        [Url, MaxLength(500)]
        public string? LinkedInUrl { get; set; }
        [MaxLength(500)]
        public string? Skills { get; set; }
        [Range(0, 80)]
        public int? YearsOfExperience { get; set; }
        [Range(0, 5)]
        public double RatingAverage { get; set; }
        public bool IsActive { get; set; } = true;
        [MaxLength(100)]
        public string? Country { get; set; }

        // >>> TAMBAHAN BARU <<<
        public string? ProfilePicturePath { get; set; }
    }

    public class TrainerUpdateDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = default!;
        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = default!;
        public string? Bio { get; set; }
        [MaxLength(30)]
        public string? Phone { get; set; }
        [Url, MaxLength(500)]
        public string? PhotoUrl { get; set; }
        [Url, MaxLength(500)]
        public string? LinkedInUrl { get; set; }
        [MaxLength(500)]
        public string? Skills { get; set; }
        [Range(0, 80)]
        public int? YearsOfExperience { get; set; }
        [Range(0, 5)]
        public double RatingAverage { get; set; }
        public bool IsActive { get; set; } = true;
        [MaxLength(100)]
        public string? Country { get; set; }

        // >>> TAMBAHAN BARU AGAR ERROR HILANG <<<
        public string? ProfilePicturePath { get; set; }
    }
}