using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using sample.Models;
using sample.Services;

namespace sample.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISenderService _senderService;
    private readonly IHttpClientFactory _clientFactory;

    public HomeController(IHttpClientFactory clientFactory,ILogger<HomeController> logger,ISenderService senderService)
    {
        _logger = logger;
        senderService = _senderService;
        _clientFactory = clientFactory;
    }

    public IActionResult Index()
    {
        Set("Id","2",30);
        HttpContext.Session.SetString("Id","1");
        return View();
    }

    public async Task<IActionResult> PrivacyAsync()
    {   
        ViewBag.Cookie = Get("Id");
        ViewBag.Session = HttpContext.Session.GetString("Id");
        var httpClient = _clientFactory.CreateClient();
        MemoryStream result = new MemoryStream();
        var query = "HaNoi";
        using (var response = await httpClient.GetAsync($"https://localhost:7102/api/Rookies/export-excel",HttpCompletionOption.ResponseContentRead))
        {
            response.EnsureSuccessStatusCode();
            result = (MemoryStream)await response.Content.ReadAsStreamAsync();
        }
         return new FileContentResult(result.ToArray(),"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
         {
             FileDownloadName = "Rookies.xlsx"
         };

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
      private void Set(string key, string value, int? expiredTime)
    {
        CookieOptions options = new CookieOptions();
        if(expiredTime.HasValue)
        {
            options.Expires = DateTime.Now.AddMinutes(expiredTime.Value);
        }else{
            options.Expires = DateTime.Now.AddSeconds(30);
        }
        Response.Cookies.Append(key,value,options);
    }

    private string Get(string key)
    {
        return Request.Cookies[key];
    }

    private void Remove(string key)
    {
        Response.Cookies.Delete(key);
    }
}
