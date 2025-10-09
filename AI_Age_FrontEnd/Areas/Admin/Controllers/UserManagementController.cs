using AI_Age_FrontEnd.Areas.Admin.Models.UserViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace AI_Age_FrontEnd.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserManagementController : Controller
    {
        private readonly HttpClient _httpClient;

        public UserManagementController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<IActionResult> Index(string searchQuery)
        {
            ViewData["CurrentFilter"] = searchQuery;

            var requestUri = "api/Users";
            if (!string.IsNullOrEmpty(searchQuery))
            {
                requestUri += $"?searchQuery={Uri.EscapeDataString(searchQuery)}";
            }

            var response = await _httpClient.GetAsync(requestUri);
            var users = new List<UserViewModel>();

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                users = JsonSerializer.Deserialize<List<UserViewModel>>(jsonString, options);
            }
            else
            {
                ViewBag.ErrorMessage = "Không thể tải danh sách người dùng từ API.";
            }
            return View(users);
        }

        public IActionResult Create()
        {
            return View(new UserCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jsonContent = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Users", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Thêm mới người dùng thành công!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorDoc = JsonDocument.Parse(errorContent);
                var message = errorDoc.RootElement.GetProperty("message").GetString();
                ModelState.AddModelError(string.Empty, message ?? "Đã có lỗi xảy ra khi thêm mới.");
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/Users/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                /* Vì backend DTO và frontend ViewModel khác nhau (RoleName vs RoleId),
                chúng ta deserialize vào một đối tượng tạm thời rồi map sang ViewModel cần cho view */
                var userFromApi = JsonSerializer.Deserialize<UserViewModel>(jsonString, options);

                // Lấy các thông tin chi tiết khác mà UserViewModel không có
                var userDetailsJson = JsonDocument.Parse(jsonString).RootElement;

                var userForEdit = new UserUpdateViewModel
                {
                    UserId = userFromApi.UserId,
                    FullName = userFromApi.FullName,
                    Username = userFromApi.Username,
                    // Chuyển đổi RoleName (string) thành RoleId (int)
                    RoleId = userFromApi.RoleName.Equals("Admin", StringComparison.OrdinalIgnoreCase) ? 1 : 3,
                    PhoneNumber = userDetailsJson.TryGetProperty("phoneNumber", out var phone) ? phone.GetString() : null,
                    DateOfBirth = userDetailsJson.TryGetProperty("dateOfBirth", out var dob) && dob.ValueKind != JsonValueKind.Null ? dob.GetDateTime() : null,
                    Gender = userDetailsJson.TryGetProperty("gender", out var gender) ? gender.GetString() : null,
                    Address = userDetailsJson.TryGetProperty("address", out var address) ? address.GetString() : null,
                };

                return View(userForEdit);
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["ErrorMessage"] = "Không tìm thấy người dùng này.";
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra, không thể lấy dữ liệu để chỉnh sửa.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserUpdateViewModel model)
        {
            if (id != model.UserId)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jsonContent = JsonSerializer.Serialize(model);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/Users/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cập nhật người dùng thành công!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                var errorDoc = JsonDocument.Parse(errorContent);
                var message = errorDoc.RootElement.GetProperty("message").GetString();
                ModelState.AddModelError(string.Empty, message ?? "Đã có lỗi xảy ra khi cập nhật.");
                return View(model);
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Users/{id}");

            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Xóa người dùng thành công!" });
            }

            return Json(new { success = false, message = "Có lỗi xảy ra, không thể xóa người dùng." });
        }
    }
}
