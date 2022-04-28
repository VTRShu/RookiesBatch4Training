using LibraryManagementBE.Repositories.DTOs;
using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Requests;

namespace LibraryManagementBE.Services;
public interface ICategoryService{
    
    Task<PagingResult<CategoryDTO>> GetCategoriesAsync(PagingRequest request);
    Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category);
    Task<CategoryDTO> UpdateCategoryAsync(CategoryDTO category, Guid id);
    Task<bool> DeleteCategoryAsync(Guid id);
    Task<CategoryDTO> GetCategoryAsync(Guid id);
    Task<List<CategoryDTO>> GetAllCategoryAsync();
}