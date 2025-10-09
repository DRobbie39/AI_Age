namespace AI_Age_BackEnd.DTOs.UserDTO.Admin
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public string RoleName { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
