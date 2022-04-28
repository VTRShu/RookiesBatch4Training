using ASP.Net_Core_API_Exer.Models;
using ASP.Net_Core_API_Exer.Models.Enums;
using ASP.Net_Core_API_Exer.Request;
using ASP.Net_Core_API_Exer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Net_Core_API_Exer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RookieController : ControllerBase
{
    private readonly IRookieService _rookieService;
    public RookieController(IRookieService rookieService)
    {
        _rookieService = rookieService;
    }
    [HttpGet("list")]
    public async Task<IActionResult> Rookies()
    {
        return Ok(await _rookieService.GetRookieListAsync());
    }
    [HttpGet("details")]
    public async Task<IActionResult> Rookie(int id)
    {
        var result = await _rookieService.GetRookieDetailsAsync(id);
        if (result != null)
        {
            return Ok(result);
        }
        return NotFound();
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    { 
        var result = await _rookieService.DeleteRookieAsync(id);
        if (result == true)
        {
            return Ok(result);
        }
        return BadRequest();
    }
    [HttpPost]
    public async Task<IActionResult> Create(RookieModel rookie)
    {
        if (ModelState.IsValid)
        {
            return Ok(await _rookieService.CreateNewRookieAsync(rookie));
        }
        return BadRequest("Something went wrong");
    }

    [HttpPut]
    public async Task<IActionResult> Edit(int id,RookieModel rookie)
    {
        if (ModelState.IsValid)
        {
            var currentRookie = await _rookieService.EditRookieAsync(id, rookie);
            if (currentRookie != null)
            {
                return Ok(currentRookie);
            }
            return BadRequest();
        }
        return BadRequest();
    }
    [HttpGet("filter")]
    public async Task<IActionResult> Filter([FromQuery]FilterRequest request)
    {
        return Ok(await _rookieService.FilterRookie(request));
    }
}