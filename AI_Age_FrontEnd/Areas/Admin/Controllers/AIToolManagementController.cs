using Microsoft.AspNetCore.Mvc;

namespace AI_Age_FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AIToolManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
