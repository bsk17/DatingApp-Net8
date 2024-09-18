using System;
using AutoMapper;
using DatingAppServer.DTOs;
using DatingAppServer.Entities;
using DatingAppServer.Extensions;

namespace DatingAppServer.Helpers;

/// <summary>
/// To map the properties of Entities to its DTOs.
/// Automapper is available in nuget.
/// As it is derived from Profile -> it will get registered in assemblies
/// </summary>
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AppUser, MemberDTO>()
            .ForMember(d => d.Age, o => o.MapFrom(s => s.DateofBirth.CalculateAge()))
            .ForMember(d => d.PhotoUrl, o =>
                o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url));

        CreateMap<Photo, PhotoDto>();
        CreateMap<MemberUpdateDTO, AppUser>();
    }
}
