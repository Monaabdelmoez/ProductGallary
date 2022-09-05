using Microsoft.AspNetCore.Mvc;

namespace ProductGallary.Controllers
{
    public class DevelopersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
