using System.Security.Claims;
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
public class UserController : ControllerBase
{
    [NonAction]
    public string GetUserRole()
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        return claimsIdentity.FindFirst(ClaimTypes.Role).Value;
    }
    [NonAction]
    public string GetSuperUserId()
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var userData = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;
        var userId = userData.Split(new[] { ';' });
        return userId[0];
    }
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    public UserController(IMapper mapper, IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(AppUserModel user)
    {
        if (ModelState.IsValid)
        {
            var userDto = _mapper.Map<AppUserDTO>(user);
            var result = await _userService.CreateUserAsync(userDto);
            if (result != null)
            {
                return Ok(_mapper.Map<AppUserModel>(result));
            }
            return BadRequest();
        }
        return BadRequest();
    }

    [HttpPut("edit")]
    [Authorize(Roles = "SuperUser,NormalUser")]
    public async Task<IActionResult> Update(Guid id, AppUserModel user)
    {
        if (ModelState.IsValid)
        {
            var userRole = GetUserRole();
            var userDTO = _mapper.Map<AppUserDTO>(user);
            var result = await _userService.UpdateUserAsync(userRole, userDTO, id);
            if (result != null)
            {
                return Ok(_mapper.Map<AppUserModel>(result));
            }
            return BadRequest();
        }
        return BadRequest();
    }
    [HttpPut("disable")]
    [Authorize(Roles = "SuperUser")]
    public async Task<IActionResult> Disable(Guid id)
    {
        return Ok(await _userService.DisableUserAsync(id));
    }
    [HttpGet]
    [Authorize(Roles = "SuperUser,NormalUser")]
    public async Task<IActionResult> UserDetails(Guid id)
    {
        var result = await _userService.GetUserDetailsAsync(id);
        if (result != null)
        {
            return Ok(_mapper.Map<AppUserModel>(result));
        }
        return BadRequest("Couldn't find user");
    }
    [HttpGet("list")]
    [Authorize(Roles = "SuperUser")]
    public async Task<IActionResult> Users( 
        [FromQuery]FilterRequest filterRequest,
        [FromQuery(Name = "pageSize")] int pageSize, 
        [FromQuery(Name = "pageIndex")] int pageIndex = 1
    )
    {
        var request = new PagingRequest
        {
            PageSize = pageSize,
            PageIndex = pageIndex
        };
        return Ok(await _userService.GetUserListAsync(request,filterRequest));
    }

}
