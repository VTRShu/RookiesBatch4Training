using Microsoft.AspNetCore.Mvc;
using ASP.NetCoreMVCEx1.Models;
using ASP.NetCoreMVCEx1.Services;

namespace ASP.NetCoreMVCEx1.Controllers
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
            var rookies = await _rookiesService.GetAllRookies();
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
            return View(ViewBag.FullName);
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
        [HttpGet("Nashtech/Rookies/Details/")]
        public async Task<IActionResult> Details(int id)
        {
            var rookie = await _rookiesService.GetRookieDetail(id);
            if (rookie == null)
            {
                return NotFound();
            }
            return View(rookie);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var deletedRookie = _rookiesService.GetById(id);
            if (deletedRookie != null)
            {
                HttpContext.Session.SetString("rookie", $"{deletedRookie.RookieId}  {deletedRookie.FullName}");
                var getSession = this.HttpContext.Session.GetString("rookie");
                TempData["DeleteSuccess"] = $"Rookie {getSession} was removed from the Rookie list successfully";
            }
            else
            {
                TempData["DeleteSuccess"] = $"The chosen rookie was already removed from the Rookie list!";
            }
            await _rookiesService.DeleteRookie(id);
            return RedirectToAction("Index");
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

        public IActionResult Edit(int id)
        {
            var currentRookie = _rookiesService.GetById(id);
            return View(currentRookie);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, RookieModel rookie)
        {
            if (ModelState.IsValid)
            {
                var result = await _rookiesService.EditRookie(id, rookie);
                if (result != null)
                {
                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            return View(rookie);
        }
        public async Task<FileContentResult> ExportExcelButton()
        {
            var wb = await _rookiesService.ExportExcel();
            using (MemoryStream stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = "Rookies.xlsx"
                };
            }
        }
    }
}
