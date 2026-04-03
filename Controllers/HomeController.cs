using Microsoft.AspNetCore.Mvc;

namespace SixBeeHealthCare.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
