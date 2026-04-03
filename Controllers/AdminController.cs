using Microsoft.AspNetCore.Mvc;

namespace SixBeeHealthCare.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
