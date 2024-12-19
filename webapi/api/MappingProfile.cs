using System;
using AutoMapper;
using Entities.Models;
using Shared.DTO;

namespace API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>()
        .ForCtorParam("FullAddress", opt => opt.MapFrom(x => $"{x.Address} {x.Country}"));
        CreateMap<Employee, EmployeeDto>();
    }
}