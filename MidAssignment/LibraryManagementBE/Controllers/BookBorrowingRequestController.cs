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
public class BookBorrowingRequestController : ControllerBase
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
    private readonly IBookBorrowingRequestService _bookBorrowingRequestService;
    private IMapper _mapper;
    public BookBorrowingRequestController(IBookBorrowingRequestService bookBorrowingRequestService,IMapper mapper)
    {
        _bookBorrowingRequestService = bookBorrowingRequestService;
        _mapper = mapper;
    }
   
    [HttpPost]
    [Authorize(Roles = "NormalUser")]
    public async Task<ActionResult> Create(BookBorrowingRequestDTO bookBorrowingRequest)
    {   
        var userId = GetSuperUserId();
        if(ModelState.IsValid)
        {
            var result = await _bookBorrowingRequestService.CreateBorrowRequestAsync(new Guid(userId),bookBorrowingRequest);
            if(result != null)
            {
                return Ok(result);
            }
            return BadRequest();
        }
        return BadRequest();
    }
    [HttpPut]
    [Authorize(Roles = "SuperUser")]
    public async Task<ActionResult> Respond(Guid requestId, string respond)
    {   
        var userId = GetSuperUserId();
        var result = await _bookBorrowingRequestService.SuperUserResponseRequestAsync(new Guid(userId),requestId,respond);
        if(result == true)
        {
            return Ok(result);
        }
        return BadRequest();
    }
    [HttpGet]
    [Authorize(Roles = "SuperUser,NormalUser")]
    public async Task<ActionResult> BorrowRequests(
    [FromQuery(Name="pageSize")] int pageSize, [FromQuery(Name ="pageIndex")] int pageIndex = 1)
    {   
        var userId = GetSuperUserId();
        var userRole = GetUserRole();
        var request = new PagingRequest
        {
            PageSize = pageSize,
            PageIndex = pageIndex
        };
        return Ok(await _bookBorrowingRequestService.GetBorrowRequestListAsync(userRole,new Guid(userId),request));
    }
}
