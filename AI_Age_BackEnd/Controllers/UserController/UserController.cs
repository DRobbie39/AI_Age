using AI_Age_BackEnd.DTOs.UserDTO;
using AI_Age_BackEnd.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace AI_Age_BackEnd.Controllers.UserController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        //[Authorize(Roles = "Admin")] // Bật dòng này để chỉ Admin mới có quyền truy cập
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
