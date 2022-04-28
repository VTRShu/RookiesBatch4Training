
using AutoMapper;
using EFCore_Ex2.Models;
using EFCore_Ex2.Repositories.DTO;
using EFCore_Ex2.Services;
using Microsoft.AspNetCore.Mvc;

namespace EFCore_Ex2.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{   
    private readonly IMapper _mapper;
    private readonly IProductService _productService;
    public ProductController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }
    [HttpGet("list")]
    public async Task<IActionResult> Products()
    {   
        var result = await _productService.GetListProductAsync();
        return Ok(result.Select(x=> _mapper.Map<ProductModel>(x)).ToList());
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductModel product)
    {   
        if(ModelState.IsValid)
        {   
            var productDto = _mapper.Map<ProductDTO>(product);
            var result = await _productService.CreateProductAsync(productDto);
            if(result != null)
            {
                return Ok(_mapper.Map<ProductModel>(result));
            }
            return BadRequest();
        }
        return BadRequest();
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await _productService.DeleteProductAsync(id));
    }
    [HttpPut]
    public async Task<IActionResult> Edit(Guid id,ProductModel product)
    {   
        if(ModelState.IsValid)
        {   
            var productDto = _mapper.Map<ProductDTO>(product);
            var result = await _productService.EditProductAsync(id, productDto);
            if(result != null)
            {
                return Ok(_mapper.Map<ProductModel>(result));
            }
            return BadRequest();
        }
        return BadRequest();
    }
    [HttpGet("detail")]
    public async Task<IActionResult> Product(Guid id)
    {
        var result = await _productService.GetProductDetailAsync(id);
        if(result != null)
        {
            return Ok(_mapper.Map<ProductModel>(result));
        }
        return BadRequest();
    }
}