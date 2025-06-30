using System.ComponentModel.DataAnnotations;

namespace AI_Age_FrontEnd.Areas.Admin.Models.AIToolCategoryViewModel
{
    public class AIToolCategoryCreateViewModel
    {
        [Required(ErrorMessage = "Tên thể loại là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên thể loại không được vượt quá 100 ký tự.")]
        [Display(Name = "Tên thể loại")]
        public string CategoryName { get; set; }

        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự.")]
        [Display(Name = "Mô tả")]
        public string? Description { get; set; }
    }
}
