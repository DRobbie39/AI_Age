using AI_Age_FrontEnd.Models.UserViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AI_Age_FrontEnd.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7022/api/"); // URL của backend API
        }

        // GET: Đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Đăng ký
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var registerDto = new
            {
                model.FullName,
                model.Username,
                model.Password
            };

            var response = await _httpClient.PostAsJsonAsync("Auth/register", registerDto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, "Đăng ký thất bại: " + error);
            return View(model);
        }

        // GET: Đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loginDto = new
            {
                model.Username,
                model.Password
            };

            var response = await _httpClient.PostAsJsonAsync("Auth/login", loginDto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, "Đăng nhập thất bại: " + error);
            return View(model);
        }
    }
}
