using System.ComponentModel.DataAnnotations;

namespace AI_Age_FrontEnd.Areas.Admin.Models.UserViewModel
{
    public class UserUpdateViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống.")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

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
