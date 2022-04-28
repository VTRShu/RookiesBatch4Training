using LibraryManagementBE.Repositories.DTOs;
using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Requests;

namespace LibraryManagementBE.Services;
public interface IUserService{
    Task<PagingResult<AppUserDTO>> GetUserListAsync(PagingRequest pageRequest,FilterRequest filterRequest = null );
    Task<AppUserDTO> CreateUserAsync(AppUserDTO user);
    Task<AppUserDTO> UpdateUserAsync(string role,AppUserDTO user, Guid id);
    Task<bool> DisableUserAsync(Guid id);
    Task<AppUserDTO> GetUserDetailsAsync(Guid id);
    Task<List<AppUserDTO>> GetAll();
}