using System.ComponentModel.DataAnnotations;

namespace AI_Age_FrontEnd.Models.UserViewModel
{
    public class ProfileViewModel
    {
        [Display(Name = "Tên tài khoản")]
        public string Username { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        public string FullName { get; set; }

        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Giới tính")]
        public string? Gender { get; set; }

        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [Display(Name = "Thay đổi ảnh đại diện")]
        public IFormFile? AvatarFile { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Display(Name = "Xác nhận mật khẩu mới")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
