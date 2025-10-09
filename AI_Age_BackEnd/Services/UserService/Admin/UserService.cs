using AI_Age_BackEnd.DTOs.UserDTO.Admin;
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;

namespace AI_Age_BackEnd.Services.UserService.Admin
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetAllUsersAsync(string? searchQuery = null)
        {
            var users = await _userRepository.GetAllAsync(searchQuery);

            return users.Select(u => new UserDto
            {
                UserId = u.UserId,
                FullName = u.FullName,
                Username = u.Username,
                PhoneNumber = u.PhoneNumber,
                DateOfBirth = u.DateOfBirth,
                Gender = u.Gender,
                Address = u.Address,
                Avatar = u.Avatar,
                RoleName = u.Role?.RoleName ?? "Chưa xác định",
                RegistrationDate = u.RegistrationDate.GetValueOrDefault()
            }).ToList();
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("Không tìm thấy người dùng.");
            }

            return new UserDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Username = user.Username,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Address = user.Address,
                Avatar = user.Avatar,
                RoleName = user.Role?.RoleName ?? "Chưa xác định",
                RegistrationDate = user.RegistrationDate.GetValueOrDefault()
            };
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto dto)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(dto.Username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Tên đăng nhập này đã tồn tại.");
            }

            var user = new User
            {
                FullName = dto.FullName,
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RoleId = dto.RoleId,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Address = dto.Address,
                RegistrationDate = DateTime.Now
            };

            await _userRepository.AddUserAsync(user);

            var createdUser = await _userRepository.GetUserByIdAsync(user.UserId);

            return new UserDto
            {
                UserId = createdUser.UserId,
                FullName = createdUser.FullName,
                Username = createdUser.Username,
                RoleName = createdUser.Role.RoleName,
                RegistrationDate = createdUser.RegistrationDate.GetValueOrDefault()
            };
        }

        public async Task<UserDto> UpdateUserAsync(UserUpdateDto dto)
        {
            var userToUpdate = await _userRepository.GetUserByIdAsync(dto.UserId);
            if (userToUpdate == null)
            {
                throw new KeyNotFoundException("Không tìm thấy người dùng để cập nhật.");
            }

            userToUpdate.FullName = dto.FullName;
            userToUpdate.RoleId = dto.RoleId;
            userToUpdate.PhoneNumber = dto.PhoneNumber;
            userToUpdate.DateOfBirth = dto.DateOfBirth;
            userToUpdate.Gender = dto.Gender;
            userToUpdate.Address = dto.Address;

            await _userRepository.UpdateUserAsync(userToUpdate);

            var updatedUser = await _userRepository.GetUserByIdAsync(dto.UserId);

            return new UserDto
            {
                UserId = updatedUser.UserId,
                FullName = updatedUser.FullName,
                Username = updatedUser.Username,
                RoleName = updatedUser.Role.RoleName,
                RegistrationDate = updatedUser.RegistrationDate.GetValueOrDefault()
            };
        }

        public async Task DeleteUserAsync(int id)
        {
            var userToDelete = await _userRepository.GetUserByIdAsync(id);
            if (userToDelete == null)
            {
                throw new KeyNotFoundException("Không tìm thấy người dùng.");
            }

            await _userRepository.DeleteUserAsync(userToDelete);
        }
    }
}
