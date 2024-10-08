﻿using AutoMapper;
using CourseTech.Domain.Dto.Review;
using CourseTech.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Mapping
{
    public class ReviewMapping : Profile
    {
        public ReviewMapping()
        {
            // To Do .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToLongDateString())) лишнее?
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.User.Login))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToLongDateString()))
                .ReverseMap();
        }
    }
}