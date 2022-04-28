using ASP.Net_Core_MVC_6._0_API_version_.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Net_Core_MVC_6._0_API_version_.Services;
public interface IRookiesService 
{
        Task<List<RookieModel>> GetAllRookie();
        Task<List<string>> GetFullNameList();
        Task<List<RookieModel>> GetMaleRookieList();
        Task<RookieModel> GetOldestRookie();
        Task<List<RookieModel>> GetRookieByBirthYear(string condition);
        Task<RookieModel> GetRookieDetail(int id);
        Task<RookieModel> CreateRookie(RookieModel rookie);
        RookieModel GetById(int id);
        Task<RookieModel> EditRookie(int id,RookieModel rookie);
        Task<bool> DeleteRookie(int id);
        Task<FileContentResult> ExportExcel();
}