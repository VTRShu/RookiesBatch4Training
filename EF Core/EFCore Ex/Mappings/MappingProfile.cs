using AutoMapper;
using EFCore_Ex.Models;
using EFCore_Ex.Repositories.Entities;

namespace EFCore_Ex.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile(){
        CreateMap<RookieEntity, RookieDTO>();
        CreateMap<RookieDTO,RookieEntity>();
        CreateMap<RookieModel,RookieDTO>();
        CreateMap<RookieDTO,RookieModel>();
    }
}