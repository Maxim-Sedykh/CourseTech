﻿using AutoMapper;
using CourseTech.Domain.Dto.Role;
using CourseTech.Domain.Entities;

namespace CourseTech.Application.Mapping;

/// <summary>
/// Настройка маппинга для сущности "Роль" в определённые DTO.
/// </summary>
public class RoleMapping : Profile
{
    public RoleMapping()
    {
        CreateMap<Role, RoleDto>();
    }
}
