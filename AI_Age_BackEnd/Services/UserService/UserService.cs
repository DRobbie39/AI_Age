using AI_Age_BackEnd.DTOs.UserDTO;
using AI_Age_BackEnd.Repositories.Interfaces;

namespace AI_Age_BackEnd.Services.UserService
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return users.Select(u => new UserDto
            {
                Id = u.UserId,
                Username = u.Username,
                FullName = u.FullName,
                RoleName = u.Role?.RoleName
            });
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                RoleName = user.Role?.RoleName
            };
        }
    }
}
