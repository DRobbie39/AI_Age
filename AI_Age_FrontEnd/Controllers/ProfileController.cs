using Microsoft.AspNetCore.Mvc;

namespace AI_Age_FrontEnd.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
