using ASP.NetCoreMVC_EX1_.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NetCoreMVC_EX1_.Services
{
    public interface IRookiesService
    {
        Task<List<RookieModel>> GetAllRookie();
        Task<List<string>> GetFullNameList();
        Task<List<RookieModel>> GetMaleRookieList();
        Task<RookieModel> GetOldestRookie();
        Task<List<RookieModel>> GetRookieByBirthYear(string condition);
        Task<RookieModel> CreateRookie(RookieModel rookie);
        Task<FileContentResult> ExportExcel();
    }
}
