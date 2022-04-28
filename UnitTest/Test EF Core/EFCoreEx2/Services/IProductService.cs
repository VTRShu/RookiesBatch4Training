using Microsoft.AspNetCore.Mvc;
using EFCore_Ex2.Repositories.Entities;
using EFCore_Ex2.Repositories.DTO;

namespace EFCore_Ex2.Services;

public interface IProductService
{
    Task<List<ProductDTO>> GetListProductAsync();
    Task<ProductDTO> CreateProductAsync(ProductDTO product);
    Task<ProductDTO> EditProductAsync(Guid id, ProductDTO product);
    Task<bool> DeleteProductAsync(Guid id);
    Task<ProductDTO> GetProductDetailAsync(Guid id);
}