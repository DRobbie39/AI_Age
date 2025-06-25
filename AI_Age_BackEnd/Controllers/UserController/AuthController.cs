using AI_Age_BackEnd.DTOs.UserDTO;
using AI_Age_BackEnd.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace AI_Age_BackEnd.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly AuthService _userService;

        public AuthController(AuthService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var user = await _userService.RegisterUserAsync(registerDto);
                return Ok(new { Message = "Đăng ký thành công", user?.UserId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var (user, token) = await _userService.LoginUserAsync(loginDto);
                return Ok(new {
                    Message = "Đăng nhập thành công",
                    user?.UserId,
                    user?.RoleId,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
