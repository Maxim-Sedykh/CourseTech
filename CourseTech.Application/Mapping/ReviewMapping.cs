﻿using AutoMapper;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Entities;

namespace CourseTech.Application.Mapping;

/// <summary>
/// Настройка маппинга для сущности "Отзыв" в определённые DTO.
/// </summary>
public class ReviewMapping : Profile
{
    public ReviewMapping()
    {
        CreateMap<Review, ReviewDto>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.User.Login))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToLongDateString()))
            .ReverseMap();
    }
}