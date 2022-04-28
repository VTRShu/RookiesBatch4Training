using AutoMapper;
using LibraryManagementBE.Repositories.DTOs;
using LibraryManagementBE.Repositories.Models;
using LibraryManagementBE.Repositories.Requests;
using LibraryManagementBE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementBE.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private IMapper _mapper;
    public CategoryController(ICategoryService categoryService,IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }
    [HttpPost]
    [Authorize(Roles = "SuperUser")]
    public async Task<IActionResult> Create(CategoryModel category)
    {
        if(ModelState.IsValid)
        {
            var categoryDto = _mapper.Map<CategoryDTO>(category);
            var result = await _categoryService.CreateCategoryAsync(categoryDto);
            if(result != null)
            {
                return Ok(_mapper.Map<CategoryModel>(result));
            }
            return BadRequest();
        }
        return BadRequest();
    }
    [HttpPut]
    [Authorize(Roles = "SuperUser")]
    public async Task<IActionResult> Update(Guid id,CategoryModel category)
    {
        if(ModelState.IsValid)
        {
            var categoryDto = _mapper.Map<CategoryDTO>(category);
            var result = await _categoryService.UpdateCategoryAsync(categoryDto,id);
            if(result != null)
            {
                return Ok(_mapper.Map<CategoryModel>(result));
            }
            return BadRequest();
        }
        return BadRequest();
    }
    [HttpDelete]
    [Authorize(Roles = "SuperUser")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await _categoryService.DeleteCategoryAsync(id));
    }
    [HttpGet]
    [Authorize(Roles = "SuperUser,NormalUser")]
    public async Task<IActionResult> Category(Guid id)
    {
        var result = await _categoryService.GetCategoryAsync(id);
        if(result != null)
        {
            return Ok(_mapper.Map<CategoryModel>(result));
        }
        return BadRequest("Couldn't find category");
    }
    [HttpGet("list")]
    [Authorize(Roles = "SuperUser,NormalUser")]
    public async Task<IActionResult> Categories(
    [FromQuery(Name="pageSize")] int pageSize, [FromQuery(Name ="pageIndex")] int pageIndex = 1)
    {
        var request = new PagingRequest
        {
            PageSize = pageSize,
            PageIndex = pageIndex
        };
        return Ok(await _categoryService.GetCategoriesAsync(request));
    }
    [HttpGet("all")]
    // [Authorize(Roles = "SuperUser,NormalUser")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _categoryService.GetAllCategoryAsync());
    }
}
