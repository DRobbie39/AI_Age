using System.ComponentModel.DataAnnotations;

namespace AI_Age_FrontEnd.Models.UserViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [RegularExpression(@"^[a-zA-Z0-9_]{3,50}$", ErrorMessage = "Tên đăng nhập chỉ được chứa chữ cái, số, dấu gạch dưới và dài từ 3 đến 50 ký tự")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
