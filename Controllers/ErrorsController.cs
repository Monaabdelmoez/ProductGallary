using Microsoft.AspNetCore.Mvc;

namespace ProductGallary.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult Index(string ErroMessage)
        {
            return View(ErroMessage);
        }
    }
}
