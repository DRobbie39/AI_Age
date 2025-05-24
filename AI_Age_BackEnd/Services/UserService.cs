using AI_Age_BackEnd.DTOs.UserDTO;
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories;

namespace AI_Age_BackEnd.Services
{
    public class UserService
    {
        private readonly AI_Age_HelpContext _context;
        private readonly UserRepository _userRepository;

        public UserService(AI_Age_HelpContext context)
        {
            _context = context;
            _userRepository = new UserRepository(_context);
        }

        public async Task<User?> RegisterUserAsync(RegisterDto registerDto)
        {
            // Kiểm tra email đã tồn tại chưa
            var existingUser = await _userRepository.GetUserByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new Exception("Email đã tồn tại.");
            }

            // Mã hóa mật khẩu
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var newUser = new User
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                Password = hashedPassword,
                RoleId = 3,
                RegistrationDate = DateTime.Now,
                Status = true
            };

            await _userRepository.AddUserAsync(newUser);
            return newUser;
        }

        public async Task<User?> LoginUserAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                throw new Exception("Email hoặc mật khẩu không đúng.");
            }

            return user;
        }
    }
}
