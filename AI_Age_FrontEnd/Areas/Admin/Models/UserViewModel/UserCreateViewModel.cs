using System.ComponentModel.DataAnnotations;

namespace AI_Age_FrontEnd.Areas.Admin.Models.UserViewModel
{
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "Họ và tên không được để trống.")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        [Display(Name = "Vai trò")]
        public int RoleId { get; set; }

        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Giới tính")]
        public string? Gender { get; set; }

        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }
    }
}
