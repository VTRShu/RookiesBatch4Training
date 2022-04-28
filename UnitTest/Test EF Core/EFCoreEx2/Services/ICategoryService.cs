using EFCore_Ex2.Repositories.DTO;
using EFCore_Ex2.Repositories.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EFCore_Ex2.Services;

public interface ICategoryService
{
    Task<List<CategoryDTO>> GetCategoryListAsync();
    Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category);
    Task<CategoryDTO> EditCategoryAsync(Guid id, CategoryDTO category);
    Task<bool> DeleteCategoryAsync(Guid id);
    Task<CategoryDTO> GetCategoryDetailAsync(Guid id);
}