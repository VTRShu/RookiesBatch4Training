using System.Globalization;
using AutoMapper;
using LibraryManagementBE.Repositories.DTOs;
using LibraryManagementBE.Repositories.EFContext;
using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Enum;
using LibraryManagementBE.Repositories.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementBE.Services.Implements;
public class UserService : IUserService
{   
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<UserService> _logger;
    private readonly LibraryManagementDBContext _libraryManagementDBContext;
    private readonly IMapper _mapper;
    public UserService(UserManager<AppUser> userManager,
    LibraryManagementDBContext libraryManagementDBContext,
    ILogger<UserService> logger,
    IMapper mapper)
    {
        _logger = logger;
        _userManager = userManager;
        _libraryManagementDBContext = libraryManagementDBContext;
        _mapper = mapper;
    }
    public AppUser GetUserById(Guid id) => _libraryManagementDBContext.AppUser.FirstOrDefault(x=>x.Id == id);
    public async Task<AppUserDTO> CreateUserAsync(AppUserDTO user)
    {
        AppUserDTO result = null;
        using var transaction = _libraryManagementDBContext.Database.BeginTransaction();
        try{
            var newUser = _mapper.Map<AppUser>(user);
            var existEmail = _libraryManagementDBContext.AppUser.FirstOrDefault(x=>x.Email.ToLower() == user.Email.ToLower());
            var existUsername = _libraryManagementDBContext.AppUser.FirstOrDefault(x=>x.UserName.ToLower() == user.UserName.ToLower());
            if(existEmail != null) return null;
            if(existUsername != null) return null;
            user.Password = user.UserName+"@"+user.Dob.ToString("ddMMyyyy", CultureInfo.InvariantCulture);
            var CreateUser = await _userManager.CreateAsync(newUser, user.Password);
            newUser.IsDisabled = false;
            await _libraryManagementDBContext.AppUser.AddAsync(newUser);
            string role = user.Type == (Role)0 ? "NormalUser" : "SuperUser";
            if(await _userManager.IsInRoleAsync(newUser, role) == false)  await _userManager.AddToRoleAsync(newUser, role);
            await _libraryManagementDBContext.SaveChangesAsync();
            await transaction.CommitAsync();
            result = _mapper.Map<AppUserDTO>(newUser);
            return result;
        }catch(Exception e){
            _logger.LogError("Couldn't Create User");
        }
        return result;
    }

    public async Task<bool> DisableUserAsync(Guid id)
    {
        var existUser = GetUserById(id);
        if(existUser != null)
        {   
            if(existUser.IsDisabled == true) return false;
            existUser.IsDisabled = true;
            await _libraryManagementDBContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<PagingResult<AppUserDTO>> GetUserListAsync(PagingRequest pageRequest,FilterRequest filterRequest = null )
    {
        var users = _libraryManagementDBContext.AppUser.Where(x=>x.IsDisabled == false);
        var result = AddFilterQuery(filterRequest, users);
        int total = await result.CountAsync();
        var data = await result.Skip((pageRequest.PageIndex -1)*pageRequest.PageSize).Take(pageRequest.PageSize)
        .Select(x=> _mapper.Map<AppUserDTO>(x)).ToListAsync();
        var pageResult = new PagingResult<AppUserDTO>()
        {
            Items = data,
            TotalRecords = total,
            PageSize = pageRequest.PageSize,
            PageIndex = pageRequest.PageIndex,
        };
        return pageResult;
    } 
    private IQueryable<AppUserDTO> AddFilterQuery(FilterRequest request, IQueryable<AppUser> userFilter)
    {
        if (!string.IsNullOrEmpty(request?.Name))
        {
            userFilter = userFilter.Where(x=>  x.FullName.ToLower().Contains(request.Name.ToLower()));
        }
        if (!string.IsNullOrEmpty(request?.Email))
        {
            userFilter = userFilter.Where(x=>  x.Email.ToLower().Contains(request.Email.ToLower()));
        }
        if (request.Gender != null)
        {
            userFilter = userFilter.Where(x=>  x.Gender == request.Gender);
        }
        if(request.Role != null)
        {
            userFilter = userFilter.Where(x=> x.Type == request.Role);
        }
        return userFilter.Select(x=> _mapper.Map<AppUserDTO>(x));
    } 

    public async Task<AppUserDTO> UpdateUserAsync(string role,AppUserDTO user, Guid id)
    {
        AppUserDTO result = null;
       
        using var transaction = _libraryManagementDBContext.Database.BeginTransaction();
        try{
            var existUser = GetUserById(id);
            if(existUser != null)
            {
                existUser.FullName = user.FullName;
                existUser.Dob = user.Dob;
                existUser.Gender = user.Gender;
                // if(role == "SuperUser")
                // {
                //     var oldRole = _userManager.GetRolesAsync(existUser).Result[0].ToString();
                //     if (await _userManager.IsInRoleAsync(existUser, oldRole))
                //     {
                //         await _userManager.RemoveFromRoleAsync(existUser, oldRole);
                //     }
                //     string newRole = user.Type == (Role)0 ? "NormalUser" : "SuperUser";
                //     if (!await _userManager.IsInRoleAsync(existUser, newRole))
                //     {
                //         await _userManager.AddToRoleAsync(existUser, newRole);
                //     }
                // }
                _libraryManagementDBContext.Entry(existUser).State = EntityState.Modified;
                await _libraryManagementDBContext.SaveChangesAsync();
                await transaction.CommitAsync();
                result = _mapper.Map<AppUserDTO>(existUser);
                return result;
            }
            return null;
        }catch(Exception e)
        {
            _logger.LogError("Couldn't Update User");
        }
        return result;
    }

    public async Task<AppUserDTO> GetUserDetailsAsync(Guid id)
    {
        var existUser = GetUserById(id);
        AppUserDTO result = null;
        if(existUser != null && existUser.IsDisabled == false)
        {   
            result = _mapper.Map<AppUserDTO>(existUser);
            return result;
        }
        return null;
    }
    public async Task<List<AppUserDTO>> GetAll()
    {
        return await _libraryManagementDBContext.AppUser.Where(x=>x.IsDisabled == false).Select(x=> _mapper.Map<AppUserDTO>(x)).ToListAsync();
    }
}