using System.Security.Claims;
using LibraryManagementBE.Repositories.Requests;
using LibraryManagementBE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementBE.Controllers;
[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{   
    [NonAction]
    public string GetSuperUserId()
    {  
      var claimsIdentity = User.Identity as ClaimsIdentity;
      var userData = claimsIdentity.FindFirst(ClaimTypes.UserData).Value;
      var userId = userData.Split(new[] { ';' });
      return userId[0];
    }
    private readonly IAuthenticationService _authenticationService;
    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _authenticationService.Authenticate(request);
        if (result == null)
        {
            return BadRequest("UserName or Password is incorrect.");
        }
        return Ok(new
        {
            token = result[0],
        });
    }
    [HttpPost("new-password")]
    [AllowAnonymous]
    public async Task<ActionResult> ChangePassword(ChangePasswordRequest request)
    {   
        var userId = GetSuperUserId();
        var result = await _authenticationService.ChangePassword(new Guid(userId),request);
        if (result == null) return BadRequest("Error !");
        else if (result == "Fail") return BadRequest("Could not change password, you may check the password format");
        else if (result == "Wrong Password") return BadRequest("Password is incorrect");
        return Ok("Your password has been changed successfully");
    }
}