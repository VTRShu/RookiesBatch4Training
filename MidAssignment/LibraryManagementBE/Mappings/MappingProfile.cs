using AutoMapper;
using LibraryManagementBE.Repositories.DTOs;
using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Models;

namespace EFCore_Ex2.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile(){
        CreateMap<AppUserDTO,AppUser>();
        CreateMap<AppUserModel,AppUserDTO>();
        CreateMap<AppUserDTO,AppUserModel>();
        CreateMap<AppUser,AppUserDTO>();

        CreateMap<CategoryDTO,CategoryEntity>();
        CreateMap<CategoryModel,CategoryDTO>();
        CreateMap<CategoryDTO,CategoryModel>();
        CreateMap<CategoryEntity,CategoryDTO>();

        CreateMap<BookDTO,BookEntity>();
        CreateMap<BookModel,BookDTO>();
        CreateMap<BookDTO,BookModel>();
        CreateMap<BookEntity,BookDTO>();
    }
}