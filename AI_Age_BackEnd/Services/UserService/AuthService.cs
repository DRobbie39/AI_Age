using AI_Age_BackEnd.DTOs.UserDTO;
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories;
using AI_Age_BackEnd.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AI_Age_BackEnd.Services.UserService
{
    public class AuthService
    {
        //private readonly AI_AgeContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<User?> RegisterUserAsync(RegisterDto registerDto)
        {
            // Kiểm tra tên đăng nhập đã tồn tại chưa
            var existingUser = await _userRepository.GetUserByUsernameAsync(registerDto.Username);
            if (existingUser != null)
            {
                throw new Exception("Tên đăng nhập đã tồn tại.");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var newUser = new User
            {
                FullName = registerDto.FullName,
                Username = registerDto.Username,
                Password = hashedPassword,
                RoleId = 3,
                RegistrationDate = DateTime.Now
            };

            await _userRepository.AddUserAsync(newUser);
            return newUser;
        }

        public async Task<(User? User, string? Token)> LoginUserAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                throw new Exception("Tên đăng nhập hoặc mật khẩu không đúng.");
            }

            var token = GenerateJwtToken(user);
            return (user, token);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim("UserId", user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.RoleId.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
