using System.ComponentModel.DataAnnotations;

namespace AI_Age_BackEnd.DTOs.UserDTO
{
    public class UserUpdateDto
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? NewPassword { get; set; }
        public IFormFile? AvatarFile { get; set; }
    }
}
