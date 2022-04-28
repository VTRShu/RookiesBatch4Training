using AutoMapper;
using EFCore_Ex2.Repositories.DTO;
using EFCore_Ex2.Repositories.EFContext;
using EFCore_Ex2.Repositories.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore_Ex2.Services.Implements;

public class ProductService : IProductService
{
    private ProductStoreDBContext _productStoreDBContext;
    private readonly ILogger<ProductService> _logger;
    private readonly IMapper _mapper;
    public ProductService(ProductStoreDBContext productStoreDBContext, ILogger<ProductService> logger, IMapper mapper)
    {
        _productStoreDBContext = productStoreDBContext;
        _logger = logger;
        _mapper = mapper;
    }
    public CategoryEntity GetCategoryByIdAsync(Guid id) => _productStoreDBContext.CategoryEntity.FirstOrDefault(x => x.CategoryId == id);
    public ProductEntity GetProductByIdAsync(Guid id) => _productStoreDBContext.ProductEntity.FirstOrDefault(x => x.ProductId == id);
    public async Task<List<ProductDTO>> GetListProductAsync()
    {
        return await _productStoreDBContext.ProductEntity.Select(x => _mapper.Map<ProductDTO>(x)).ToListAsync();
    }

    public async Task<ProductDTO> CreateProductAsync(ProductDTO product)
    {
        ProductDTO result = null;
        using var transaction = _productStoreDBContext.Database.BeginTransaction();
        try
        {
            var existCategory = GetCategoryByIdAsync((Guid)product.CategoryId);
            if (existCategory != null)
            {
                var newProduct = _mapper.Map<ProductEntity>(product);
                await _productStoreDBContext.ProductEntity.AddAsync(newProduct);
                await _productStoreDBContext.SaveChangesAsync();
                await transaction.CommitAsync();
                result = _mapper.Map<ProductDTO>(newProduct);
                return result;
            }
            return result;
        }
        catch (Exception)
        {
            _logger.LogError("Something went wrong!");
        }
        return result;
    }

    public async Task<ProductDTO> EditProductAsync(Guid id, ProductDTO product)
    {   
        ProductDTO result = null;
        using var transaction = _productStoreDBContext.Database.BeginTransaction();
        try
        {
            var existProduct = GetProductByIdAsync(id);
            if(existProduct != null)
            {
                existProduct.ProductName = product.ProductName;
                existProduct.Manufacture = product.Manufacture;
                existProduct.CategoryId = product.CategoryId;
                _productStoreDBContext.Entry(existProduct).State = EntityState.Modified;
                await _productStoreDBContext.SaveChangesAsync();
                await transaction.CommitAsync();
                result = _mapper.Map<ProductDTO>(existProduct);
                return result;
            }
            return result;
        }catch (Exception)
        {
            _logger.LogError("Something went wrong!");
        }
        return result;
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        using var transaction = _productStoreDBContext.Database.BeginTransaction();
        try
        {
            var existProduct = GetProductByIdAsync(id);
            if(existProduct != null)
            {
                _productStoreDBContext.ProductEntity.Remove(existProduct);
                await _productStoreDBContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            return false;
        }catch (Exception)
        {
            _logger.LogError("Something went wrong!");
        }
        return false;
    }

    public async Task<ProductDTO> GetProductDetailAsync(Guid id)
    {
        var existProduct = GetProductByIdAsync(id);
        if(existProduct != null)
        {
            return await Task.FromResult(_mapper.Map<ProductDTO>(existProduct));
        }
        return null;
    }
}