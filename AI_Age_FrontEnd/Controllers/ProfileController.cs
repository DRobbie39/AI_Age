using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace AI_Age_FrontEnd.Controllers
{
    public class ProfileController : Controller
    {
        private readonly HttpClient _httpClient; 
        private readonly IConfiguration _configuration;

        public ProfileController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewBag.ApiBaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl");

            return View();
        }

        public IActionResult SavedLessons()
        {
            return View();
        }
    }
}
