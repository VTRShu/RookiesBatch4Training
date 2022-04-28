using AutoMapper;
using LibraryManagementBE.Repositories.DTOs;
using LibraryManagementBE.Repositories.EFContext;
using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Enum;
using LibraryManagementBE.Repositories.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementBE.Services.Implements;
public class CategoryService : ICategoryService
{   
    private readonly ILogger<CategoryService> _logger;
    private readonly LibraryManagementDBContext _libraryManagementDBContext;
    private readonly IMapper _mapper;
    public CategoryService(ILogger<CategoryService> logger, LibraryManagementDBContext libraryManagementDBContext, IMapper mapper)
    {
        _logger = logger;
        _libraryManagementDBContext = libraryManagementDBContext;
        _mapper = mapper;
    }
    public CategoryEntity GetCategoryById(Guid id) => _libraryManagementDBContext.CategoryEntity.FirstOrDefault(x=>x.Id == id);
    public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category)
    {
        CategoryDTO result = null;
        using var transaction = _libraryManagementDBContext.Database.BeginTransaction();
        try{
            var existedCategory = _libraryManagementDBContext.CategoryEntity.FirstOrDefault(x=>x.CategoryName.ToLower() == category.CategoryName.ToLower());
            if(existedCategory == null)
            {   
                category.CreatedAt = DateTime.Now;
                var newCategory = _mapper.Map<CategoryEntity>(category);
                await _libraryManagementDBContext.CategoryEntity.AddAsync(newCategory);
                await _libraryManagementDBContext.SaveChangesAsync();
                await transaction.CommitAsync();
                result = _mapper.Map<CategoryDTO>(newCategory);
                return result;
            }
            return result;
        }catch(Exception e)
        {
            _logger.LogError("Something went wrong!");
        }
        return result;
    }

    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
        var existedCategory = GetCategoryById(id);
        if(existedCategory != null)
        {
            _libraryManagementDBContext.CategoryEntity.Remove(existedCategory);
            await _libraryManagementDBContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<PagingResult<CategoryDTO>> GetCategoriesAsync(PagingRequest request)
    {
        var categories = _libraryManagementDBContext.CategoryEntity.Select(x => _mapper.Map<CategoryDTO>(x));
        int total = await categories.CountAsync();
        var data = await categories.Skip((request.PageIndex -1) * request.PageSize).Take(request.PageSize)
        .Select(x => _mapper.Map<CategoryDTO>(x)).ToListAsync();
        var pageResult = new PagingResult<CategoryDTO>()
        {
            Items = data,
            TotalRecords = total,
            PageSize = request.PageSize,
            PageIndex = request.PageIndex
        };
        return pageResult;
    }

    public async Task<CategoryDTO> GetCategoryAsync(Guid id)
    {
        var existedCategory = GetCategoryById(id);
        if(existedCategory != null){
             return await Task.FromResult(_mapper.Map<CategoryDTO>(existedCategory));
        }
        return null;
    }

    public async Task<CategoryDTO> UpdateCategoryAsync(CategoryDTO category, Guid id)
    {   
        CategoryDTO result = null;
        using var transaction = _libraryManagementDBContext.Database.BeginTransaction();
        try{
            var existedCategory = GetCategoryById(id);
            if(existedCategory != null)
            {
                existedCategory.CategoryName = category.CategoryName;
                _libraryManagementDBContext.Entry(existedCategory).State = EntityState.Modified;
                await _libraryManagementDBContext.SaveChangesAsync();
                await transaction.CommitAsync();
                result = _mapper.Map<CategoryDTO>(existedCategory);
                return result;
            }
        }catch(Exception ex)
        {
            _logger.LogError("Something went wrong!");
        }
        return result;
    }

    public async Task<List<CategoryDTO>> GetAllCategoryAsync()
    {
         return await _libraryManagementDBContext.CategoryEntity.Select(x=> _mapper.Map<CategoryDTO>(x)).ToListAsync();
    }
}