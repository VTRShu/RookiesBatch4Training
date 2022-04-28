using AutoMapper;
using EFCore_Ex2.Models;
using EFCore_Ex2.Repositories.DTO;
using EFCore_Ex2.Services;
using Microsoft.AspNetCore.Mvc;

namespace EFCore_Ex2.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{      
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    public CategoryController(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }
    [HttpGet("list")]
    public async Task<ActionResult> Categories()
    {   
        var result = await _categoryService.GetCategoryListAsync();
        return Ok(result.Select(x=> _mapper.Map<CategoryModel>(x)).ToList());
    }
    [HttpPost]
    public async Task<ActionResult> CreateNew(CategoryModel category)
    {    var categoryDto = _mapper.Map<CategoryDTO>(category);
        if(ModelState.IsValid)
        {   
            var result = await _categoryService.CreateCategoryAsync(categoryDto);
            if(result != null)
            {
                return Ok(_mapper.Map<CategoryModel>(result));
            }
        }
        return BadRequest();
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await _categoryService.DeleteCategoryAsync(id));
    }
    [HttpPut]
    public async Task<IActionResult> Edit(Guid id, CategoryModel category)
    {   
        if(ModelState.IsValid)
        {   
            var categoryDto = new CategoryDTO(){
                CategoryName = category.CategoryName
            };
            var result = await _categoryService.EditCategoryAsync(id, categoryDto);
            if(result != null)
            {
                return Ok(_mapper.Map<CategoryModel>(result));
            }
        }
        return BadRequest();
    }
    [HttpGet("detail")]
    public async Task<IActionResult> Category(Guid id)
    {
        var result = await _categoryService.GetCategoryDetailAsync(id);
        if(result != null)
        {
            return Ok(_mapper.Map<CategoryModel>(result));
        }
        return BadRequest();
    }
}