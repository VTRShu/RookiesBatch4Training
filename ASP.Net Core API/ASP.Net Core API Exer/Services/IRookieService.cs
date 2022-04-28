using ASP.Net_Core_API_Exer.Models;
using ASP.Net_Core_API_Exer.Models.Enums;
using ASP.Net_Core_API_Exer.Request;

namespace ASP.Net_Core_API_Exer.Services;
public interface IRookieService
{
    Task<List<RookieModel>> GetRookieListAsync();
    Task<RookieModel> CreateNewRookieAsync(RookieModel rookie);
    Task<RookieModel> EditRookieAsync(int id, RookieModel rookie);
    Task<bool> DeleteRookieAsync(int id);
    Task<RookieModel> GetRookieDetailsAsync(int id);
    Task<List<RookieModel>> FilterRookie(FilterRequest request = null);
}