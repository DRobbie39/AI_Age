using Microsoft.AspNetCore.Mvc;

namespace AI_Age_FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleCategoryManagementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            return View();
        }
    }
}
