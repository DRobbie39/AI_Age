using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AI_Age_FrontEnd.Areas.Admin.Models.AIToolViewModel
{
    public class AIToolUpdateViewModel
    {
        public int ToolID { get; set; }

        [Required(ErrorMessage = "Tên công cụ là bắt buộc.")]
        [StringLength(100)]
        [Display(Name = "Tên công cụ")]
        public string ToolName { get; set; }

        [Required(ErrorMessage = "Mô tả là bắt buộc.")]
        [StringLength(500)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Url(ErrorMessage = "URL Website không hợp lệ.")]
        [Display(Name = "URL Website")]
        public string? WebsiteURL { get; set; }

        [Display(Name = "Thể loại")]
        public int? CategoryID { get; set; }

        [Display(Name = "Logo")]
        public IFormFile? Logo { get; set; }

        public string? ExistingLogoUrl { get; set; }

        // Dùng để đổ dữ liệu vào dropdownlist
        public List<SelectListItem>? Categories { get; set; }
    }
}
