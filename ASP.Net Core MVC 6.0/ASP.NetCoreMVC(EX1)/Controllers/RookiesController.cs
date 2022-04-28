using Microsoft.AspNetCore.Mvc;
using ASP.NetCoreMVC_EX1_.Models;
using ASP.NetCoreMVC_EX1_.Services;

namespace ASP.NetCoreMVC_EX1_.Controllers
{
    public class RookiesController : Controller
    {
        private readonly IRookiesService _rookiesService;
        public RookiesController(IRookiesService rookiesService)
        {
            _rookiesService = rookiesService;
        }

        public async Task<IActionResult> Index()
        {
            var rookies = await _rookiesService.GetAllRookie();
            return View(rookies);
        }
        public async Task<IActionResult> ViewMaleRookies()
        {
            var rookies = await _rookiesService.GetMaleRookieList();
            return View(rookies);
        }

        public async Task<IActionResult> ViewFullName()
        {
            var rookies = await _rookiesService.GetFullNameList();
            ViewBag.FullName = rookies;
            return View();
        }

        public async Task<IActionResult> ViewOldestRookie()
        {
            var oldestRookie = await _rookiesService.GetOldestRookie();
            return View(oldestRookie);
        }

        [HttpGet("Nashtech/Rookies/ViewByBirthYear/")]
        public async Task<IActionResult> ViewByBirthYear(string condition)
        {
            var rookies = await _rookiesService.GetRookieByBirthYear(condition);
            return View(rookies);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RookieModel rookie)
        {
            if (ModelState.IsValid)
            {
                var result = await _rookiesService.CreateRookie(rookie);
                return RedirectToAction("Index");
            }
            return View(rookie);
        }

        public async Task<FileContentResult> ExportExcelButton()
        {
            return await _rookiesService.ExportExcel();
        }
    }
}
