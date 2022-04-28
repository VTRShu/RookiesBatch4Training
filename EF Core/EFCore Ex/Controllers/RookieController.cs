using AutoMapper;
using EFCore_Ex.Models;
using EFCore_Ex.Repositories.Entities;
using EFCore_Ex.Services;
using Microsoft.AspNetCore.Mvc;

namespace EFCore_Ex.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RookieController : ControllerBase
{
    private readonly IRookieService _rookieService;
    private readonly IMapper _mapper;
    public RookieController(IRookieService rookieService,IMapper mapper)
    {
        _rookieService = rookieService;
        _mapper = mapper;
    }
    [HttpGet("list")]
    public async Task<IActionResult> Rookies()
    {
        return Ok(await _rookieService.GetRookieListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Create(RookieModel rookie)
    {   
        if(ModelState.IsValid)
        {
            var rookieDto = _mapper.Map<RookieDTO>(rookie);
            return Ok(await _rookieService.CreateRookieAsync(rookieDto));
        }
        return BadRequest();
    }
    [HttpGet("details")]
    public async Task<IActionResult> Rookie(Guid id)
    {
        var result = await _rookieService.GetRookieDetailsAsync(id);
        if(result != null)
        {
            return Ok(result);
        }
        return NotFound();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _rookieService.DeleteRookieAsync(id);
        if (result == true)
        {
            return Ok(result);
        }
        return BadRequest();
    }
    [HttpPut]
    public async Task<IActionResult> Edit(Guid id,RookieModel rookie)
    {
        if(ModelState.IsValid)
        {   
            var rookieDto = _mapper.Map<RookieDTO>(rookie);
            var currentRookie = await _rookieService.EditRookieAsync(id,rookieDto);
            if(currentRookie != null)
            {
                return Ok(currentRookie);
            }
            return BadRequest();
        }
        return BadRequest();
    }
   
}
