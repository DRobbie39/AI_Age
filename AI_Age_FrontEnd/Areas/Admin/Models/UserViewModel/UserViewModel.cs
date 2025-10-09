using System.ComponentModel.DataAnnotations;

namespace AI_Age_FrontEnd.Areas.Admin.Models.UserViewModel
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string? Avatar { get; set; }

        [Display(Name = "Vai trò")]
        public string RoleName { get; set; }

        [Display(Name = "Ngày đăng ký")]
        public DateTime RegistrationDate { get; set; }
    }
}
