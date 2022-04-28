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
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    private IMapper _mapper;
    public BookController(IBookService bookService,IMapper mapper)
    {
        _bookService = bookService;
        _mapper = mapper;
    }
    [HttpGet("abc")]
    public IActionResult abc()
    {
        return Ok("alll");
    }
    [HttpPost]
    [Authorize(Roles = "SuperUser")]
    public async Task<IActionResult> Create(BookModel book)
    {
        if(ModelState.IsValid)
        {
            var bookDto = _mapper.Map<BookDTO>(book);
            var result = await _bookService.CreateBookAsync(bookDto);
            if(result != null)
            {
                return Ok(_mapper.Map<BookModel>(result));
            }
            return BadRequest();
        }
        return BadRequest();
    }
    [HttpPut]
    [Authorize(Roles = "SuperUser")]
    public async Task<IActionResult> Update(Guid id,BookModel book)
    {
        if(ModelState.IsValid)
        {
            var bookDto = _mapper.Map<BookDTO>(book);
            var result = await _bookService.UpdateBookAsync(bookDto,id);
            if(result != null)
            {
                return Ok(_mapper.Map<BookModel>(result));
            }
            return BadRequest();
        }
        return BadRequest();
    }
    [HttpDelete]
    [Authorize(Roles = "SuperUser")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await _bookService.DeleteBookAsync(id));
    }
    [HttpGet]
    [Authorize(Roles = "SuperUser,NormalUser")]
    public async Task<IActionResult> Book(Guid id)
    {
        var result = await _bookService.GetBookAsync(id);
        if(result != null)
        {
            return Ok(_mapper.Map<BookModel>(result));
        }
        return BadRequest("Couldn't find book");
    }
    [HttpGet("list")]
    public async Task<IActionResult> Books(
    [FromQuery(Name="pageSize")] int pageSize, [FromQuery(Name ="pageIndex")] int pageIndex = 1)
    {
        var request = new PagingRequest
        {
            PageSize = pageSize,
            PageIndex = pageIndex
        };
        return Ok(await _bookService.GetBooksAsync(request));
    }
    [HttpGet("all")]
    //[Authorize(Roles = "SuperUser,NormalUser")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _bookService.GetAllBookAsync());
    }
}
