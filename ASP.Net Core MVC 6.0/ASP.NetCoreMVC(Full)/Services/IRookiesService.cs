using ASP.NetCoreMVCEx1.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NetCoreMVCEx1.Services
{
    public interface IRookiesService
    {
        Task<List<RookieModel>> GetAllRookies();
        Task<List<string>> GetFullNameList();
        Task<List<RookieModel>> GetMaleRookieList();
        Task<RookieModel> GetOldestRookie();
        Task<List<RookieModel>> GetRookieByBirthYear(string condition);
        Task<RookieModel> GetRookieDetail(int id);
        Task<RookieModel> CreateRookie(RookieModel rookie);
        RookieModel GetById(int id);
        Task<RookieModel> EditRookie(int id,RookieModel rookie);
        Task<bool> DeleteRookie(int id);
        Task<XLWorkbook> ExportExcel();
    }
}
