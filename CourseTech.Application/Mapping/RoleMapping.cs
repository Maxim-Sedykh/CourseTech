using AutoMapper;
using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Entities;

namespace CourseTech.Application.Mapping
{
    public class RoleMapping : Profile
    {
        public RoleMapping()
        {
            CreateMap<Role, RoleDto>();
        }
    }
}
