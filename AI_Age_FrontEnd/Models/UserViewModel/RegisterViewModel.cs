using System.ComponentModel.DataAnnotations;

namespace AI_Age_FrontEnd.Models.UserViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Họ và tên là bắt buộc")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [RegularExpression(@"^[a-zA-Z0-9_]{3,50}$", ErrorMessage = "Tên đăng nhập chỉ được chứa chữ cái, số, dấu gạch dưới và dài từ 3 đến 50 ký tự")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
            ErrorMessage = "Mật khẩu phải dài ít nhất 8 ký tự, chứa ít nhất một chữ hoa, một chữ thường và một chữ số.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Bạn phải đồng ý với điều khoản")]
        public bool AgreeTerms { get; set; }
    }
}
