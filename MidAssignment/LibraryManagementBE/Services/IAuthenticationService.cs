
using LibraryManagementBE.Repositories.Requests;


namespace LibraryManagementBE.Services;

public interface IAuthenticationService
{
    Task<string[]> Authenticate(LoginRequest request);
    Task<string> ChangePassword(Guid userId,ChangePasswordRequest request);
}

