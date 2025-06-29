using Microsoft.AspNetCore.Mvc;

namespace AI_Age_FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AIToolCategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
