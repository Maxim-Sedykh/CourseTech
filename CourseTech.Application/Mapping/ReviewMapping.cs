using AutoMapper;
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
            CreateMap<Review, ReviewDto>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.User.Login, opt => opt.MapFrom(src => src.Login))
                .ForMember(dest => dest.ReviewText, opt => opt.MapFrom(src => src.Text))
                .ForMember(dest => dest.CreatedAt.ToLongDateString(), opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}