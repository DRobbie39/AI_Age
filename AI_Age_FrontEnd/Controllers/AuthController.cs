using AI_Age_FrontEnd.Models.UserViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AI_Age_FrontEnd.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7022/api/");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Vui lòng kiểm tra lại thông tin nhập vào.";
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
                ViewBag.Success = true;
                return View(model);
            }
            else
            {
                ViewBag.Error = true;
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Vui lòng kiểm tra lại thông tin nhập vào.";
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
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

                ViewBag.Token = result.ContainsKey("token") ? result["token"].ToString() : null;
                ViewBag.UserId = result.ContainsKey("userId") ? result["userId"].ToString() : null;
                ViewBag.Username = model.Username;

                return View("Login");
            }
            else
            {
                ViewBag.Error = true;
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
