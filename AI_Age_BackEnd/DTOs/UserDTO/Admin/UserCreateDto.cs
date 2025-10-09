using System.ComponentModel.DataAnnotations;

namespace AI_Age_BackEnd.DTOs.UserDTO.Admin
{
    public class UserCreateDto
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
    }
}
