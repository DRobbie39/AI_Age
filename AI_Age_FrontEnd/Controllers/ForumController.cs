using Microsoft.AspNetCore.Mvc;

namespace AI_Age_FrontEnd.Controllers
{
    public class ForumController : Controller
    {
        private readonly IConfiguration _configuration;

        public ForumController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.ApiBaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");
            return View();
        }
    }
}
