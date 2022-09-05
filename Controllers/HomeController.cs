using Microsoft.AspNetCore.Mvc;
using ProductGallary.Models;
using System.Diagnostics;
using ProductGallary.TDO;
using Microsoft.AspNetCore.Identity;

namespace ProductGallary.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<ApplicationUser> userManger;
        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManger)
        {
            _logger = logger;
            this.userManger = userManger;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}