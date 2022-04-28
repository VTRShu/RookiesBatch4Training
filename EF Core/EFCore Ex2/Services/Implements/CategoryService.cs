using AutoMapper;
using EFCore_Ex2.Repositories.DTO;
using EFCore_Ex2.Repositories.EFContext;
using EFCore_Ex2.Repositories.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore_Ex2.Services.Implements;

public class CategoryService : ICategoryService
{   
    private readonly ProductStoreDBContext _productStoreDBContext;
    private readonly ILogger<CategoryService> _logger;
    private readonly IMapper _mapper;
    public CategoryService(ProductStoreDBContext productStoreDBContext,ILogger<CategoryService> logger, IMapper mapper)
    {
        _productStoreDBContext = productStoreDBContext;
        _logger = logger;
        _mapper = mapper;
    }
    public CategoryEntity GetCategoryByIdAsync(Guid id) => _productStoreDBContext.CategoryEntity.FirstOrDefault(x => x.CategoryId == id);
    public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category)
    {
        CategoryDTO result = null;
        using var transaction = _productStoreDBContext.Database.BeginTransaction();
        try{
            var newCategory = _mapper.Map<CategoryEntity>(category);
            await _productStoreDBContext.CategoryEntity.AddAsync(newCategory);
            await _productStoreDBContext.SaveChangesAsync();
            await transaction.CommitAsync();
            result = _mapper.Map<CategoryDTO>(newCategory);
            return result;
        }catch(Exception)
        {
            _logger.LogError("Something went wrong!");
        }
        return result;
    }

    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
        using var transaction = _productStoreDBContext.Database.BeginTransaction();
        try{
            var existCategory = GetCategoryByIdAsync(id);
            if(existCategory != null)
            {
                _productStoreDBContext.CategoryEntity.Remove(existCategory);
                await _productStoreDBContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            return false;
        }catch(Exception)
        {
            _logger.LogError("Something went wrong!");
        }
        return false;
    }

    public async Task<CategoryDTO> EditCategoryAsync(Guid id, CategoryDTO category)
    {   
        CategoryDTO result = null;
        using var transaction = _productStoreDBContext.Database.BeginTransaction();
        try
        {
            var existCategory = GetCategoryByIdAsync(id);
            if(existCategory != null)
            {
                existCategory.CategoryName = category.CategoryName;
                _productStoreDBContext.Entry(existCategory).State = EntityState.Modified;
                await _productStoreDBContext.SaveChangesAsync();
                await transaction.CommitAsync(); 
                result = _mapper.Map<CategoryDTO>(existCategory);
                return result;
            }
            return result;
        }catch(Exception)
        {
            _logger.LogError("Something went wrong!");
        }
        return result;
    }

    public async Task<CategoryDTO> GetCategoryDetailAsync(Guid id)
    {
        var existCategory = GetCategoryByIdAsync(id);
        if(existCategory != null)
        {   
            return await Task.FromResult(_mapper.Map<CategoryDTO>(existCategory));
        }
        return null;
    }

    public async Task<List<CategoryDTO>> GetCategoryListAsync()
    {
        return await _productStoreDBContext.CategoryEntity
        .Select(x => _mapper.Map<CategoryDTO>(x)).ToListAsync();
    }
}