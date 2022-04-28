using ASP.Net_Core_MVC_6._0_API_version_.Models;
using ASP.Net_Core_MVC_6._0_API_version_.Services;
using Microsoft.AspNetCore.Mvc;
namespace ASP.Net_Core_MVC_6._0_API_version_.Controllers;
[ApiController]
[Route("api/[controller]/")]
public class RookiesController : ControllerBase
{
    private readonly IRookiesService _rookiesService;
    public RookiesController(IRookiesService rookiesService)
    {
        _rookiesService = rookiesService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<List<RookieModel>>> GetListRookies()
    {

        return Ok(await _rookiesService.GetAllRookie());
    }

    [HttpGet("fullname-list")]
    public async Task<ActionResult<List<string>>> GetFullNameList()
    {
        return Ok(await _rookiesService.GetFullNameList());
    }

    [HttpGet("male-list")]
    public async Task<ActionResult<List<RookieModel>>> GetMaleList()
    {
        return Ok(await _rookiesService.GetMaleRookieList());
    }

    [HttpGet("oldest")]
    public async Task<ActionResult<RookieModel>> GetOldestRookie()
    {
        return Ok(await _rookiesService.GetOldestRookie());
    }

    [HttpGet("rookies-by-birthyear/{condition}")]
    public async Task<ActionResult<List<RookieModel>>> GetRookiesByBirthYear(string condition)
    {
        return Ok(await _rookiesService.GetRookieByBirthYear(condition));
    }

    [HttpGet("detail/{id}")]
    public async Task<ActionResult<RookieModel>> GetRookieById(int id)
    {
        var result = await _rookiesService.GetRookieDetail(id);
        if (result == null)
        {
            return BadRequest("Not found rookie!");
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<RookieModel>> CreateNewRookie(RookieModel rookie)
    {
        if (ModelState.IsValid)
        {
            var newRookie = await _rookiesService.CreateRookie(rookie);
            return Ok(newRookie);
        }
        return BadRequest();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RookieModel>> EditCurrentRookie(int id, RookieModel rookie)
    {
        if (ModelState.IsValid)
        {
            var currentRookie = await _rookiesService.EditRookie(id, rookie);
            if (currentRookie == null)
            {
                return BadRequest("Not found!");
            }
            return Ok(currentRookie);
        }
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCurrentRookie(int id)
    {
        var result = await _rookiesService.DeleteRookie(id);
        if(result == true)
        {
            return Ok(result);
        }
        return BadRequest("Rookie is already deleted!");
    }

    [HttpGet("export-excel")]
    public async Task<FileContentResult> ExportRookieExcelFile()
    {
        return await _rookiesService.ExportExcel();
    }
}