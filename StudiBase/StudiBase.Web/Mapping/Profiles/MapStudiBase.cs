using AutoMapper;
using StudiBase.Shared.Contracts.DTOs;
using StudiBase.Shared.Domain.Entities;
using StudiBase.Shared.Domain.Enums;

namespace StudiBase.Web.Mapping.Profiles
{
    public class MapStudiBase : Profile
    {
        public MapStudiBase()
        {
            // Trainer
            CreateMap<Trainer, TrainerDto>();
            CreateMap<TrainerCreateDto, Trainer>();
            CreateMap<TrainerUpdateDto, Trainer>();

            // Course -> DTO (enum to string, include TrainerName)
            CreateMap<Course, CourseDto>()
                .ForMember(d => d.Level, o => o.MapFrom(s => s.Level.ToString()))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.Visibility, o => o.MapFrom(s => s.Visibility.ToString()))
                .ForMember(d => d.TrainerName, o => o.MapFrom(s => s.Trainer != null ? s.Trainer.Name : null))
                .ForMember(d => d.ThumbnailImagePath, o => o.MapFrom(s => s.thumbnailImagePath));

            // Create/Update DTO -> Course (string to enum with fallback)
            CreateMap<CourseCreateDto, Course>()
                .ForMember(d => d.Level, o => o.MapFrom(s => ParseEnum<CourseLevel>(s.Level, CourseLevel.Beginner)))
                .ForMember(d => d.Status, o => o.MapFrom(s => ParseEnum<CourseStatus>(s.Status, CourseStatus.Draft)))
                .ForMember(d => d.Visibility, o => o.MapFrom(s => ParseEnum<CourseVisibility>(s.Visibility, CourseVisibility.Public)));

            CreateMap<CourseUpdateDto, Course>()
                .ForMember(d => d.Level, o => o.MapFrom(s => ParseEnum<CourseLevel>(s.Level, CourseLevel.Beginner)))
                .ForMember(d => d.Status, o => o.MapFrom(s => ParseEnum<CourseStatus>(s.Status, CourseStatus.Draft)))
                .ForMember(d => d.Visibility, o => o.MapFrom(s => ParseEnum<CourseVisibility>(s.Visibility, CourseVisibility.Public)));

            // CourseMaterial -> DTO
            CreateMap<CourseMaterial, CourseMaterialDto>()
                .ForMember(d => d.FileType, o => o.MapFrom(s => s.FileTypeMaterial.ToString()))
                .ForMember(d => d.UploadedByTrainerName, o => o.MapFrom(s => s.Trainer != null ? s.Trainer.Name : null));
        }

        private static TEnum ParseEnum<TEnum>(string? value, TEnum fallback)
            where TEnum : struct, System.Enum
        {
            if (!string.IsNullOrWhiteSpace(value) && System.Enum.TryParse<TEnum>(value, true, out var parsed))
            {
                return parsed;
            }
            return fallback;
        }
    }
}
