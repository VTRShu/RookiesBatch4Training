using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryManagementBE.Repositories.EFContext;
using LibraryManagementBE.Repositories.Entities;
using LibraryManagementBE.Repositories.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagementBE.Services.Implements;
public class AuthenticationService : IAuthenticationService
{   
    private readonly UserManager<AppUser>? _userManager;
    private readonly SignInManager<AppUser>? _signInManager;
    private readonly RoleManager<AppRole>? _roleManager;
    private readonly IConfiguration? _config;
    private readonly LibraryManagementDBContext? _libraryManagementDBContext;
    private ILogger<AuthenticationService>? _logger;
    public AuthenticationService(UserManager<AppUser> userManager,
         SignInManager<AppUser> signInManager,
         RoleManager<AppRole> roleManager,
         IConfiguration config,
         LibraryManagementDBContext libraryManagementDBContext,
         ILogger<AuthenticationService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _libraryManagementDBContext = libraryManagementDBContext;
            _logger = logger;
        }
    public async Task<string> GeneratedToken(AppUser user, DateTime expiresDate)
    {
        string result = null;
        user = await _userManager.FindByNameAsync(user.UserName);
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new[]
        {
            new Claim(ClaimTypes.Role, string.Join(";", roles)),
            new Claim(ClaimTypes.UserData, string.Join(";",user.Id,user.FullName)),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(_config["Tokens:Issuer"],
            _config["Tokens:Issuer"],
            claims,
            expires:expiresDate,
            signingCredentials: credentials); 
        result =  new JwtSecurityTokenHandler().WriteToken(token);
        return result;
    }
    public async Task<string[]> Authenticate(LoginRequest request)
    {   
        var result = new string[1];
        using var transaction = _libraryManagementDBContext?.Database.BeginTransaction();
        try{
            var user = await _userManager.FindByNameAsync(request.UserName);
            if(user == null || user.IsDisabled == true) throw new Exception("Can't find UserName");
            var login = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if(!login.Succeeded) return null;
            if(request.RememberMe == true)
            {
                result[0]= await GeneratedToken(user,DateTime.UtcNow.AddDays(7));
            }else{
                result[0]= await GeneratedToken(user,DateTime.UtcNow.AddMinutes(30));
            }
            await _libraryManagementDBContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }catch(Exception ex)
        {
            _logger.LogError("Couldn't connect database");
        }
        return result;
    }
    public AppUser GetById(Guid? id) => _libraryManagementDBContext.AppUser.FirstOrDefault(u => u.Id == id && u.IsDisabled == false);
    public async Task<string> ChangePassword(Guid userId,ChangePasswordRequest request)
    {
        var existUser = GetById(userId);
        if(existUser != null)
        {
            var result = _userManager.PasswordHasher.VerifyHashedPassword(existUser, existUser.PasswordHash,request.OldPassword);
            if(result != PasswordVerificationResult.Success)
            {
                return"Wrong Password";
            }
            var changePassword = await _userManager.ChangePasswordAsync(existUser,request.OldPassword,request.NewPassword);
            if(changePassword.Succeeded) return "Success";
            return "Fail";
        }
        return null;
    }
   
}