using AutoMapper;
using EFCore_Ex2.Models;
using EFCore_Ex2.Repositories.DTO;
using EFCore_Ex2.Repositories.Entities;

namespace EFCore_Ex2.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile(){
        CreateMap<CategoryModel,CategoryDTO>();
        CreateMap<CategoryEntity,CategoryDTO>();
        CreateMap<CategoryDTO,CategoryEntity>();
        CreateMap<CategoryDTO,CategoryModel>();

        CreateMap<ProductModel,ProductDTO>();
        CreateMap<ProductEntity,ProductDTO>();
        CreateMap<ProductDTO,ProductEntity>();
        CreateMap<ProductDTO,ProductModel>();
    }
}