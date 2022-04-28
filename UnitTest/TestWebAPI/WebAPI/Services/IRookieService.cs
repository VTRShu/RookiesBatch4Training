using WebAPI.Models;
using WebAPI.Models.Enums;
using WebAPI.Request;

namespace WebAPI.Services;
public interface IRookieService
{
    Task<List<RookieModel>> GetRookieListAsync();
    Task<RookieModel> CreateNewRookieAsync(RookieModel rookie);
    Task<RookieModel> EditRookieAsync(int id, RookieModel rookie);
    Task<bool> DeleteRookieAsync(int id);
    Task<RookieModel> GetRookieDetailsAsync(int id);
    Task<List<RookieModel>> FilterRookie(FilterRequest request = null);
}