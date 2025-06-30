using System.ComponentModel.DataAnnotations;

namespace AI_Age_FrontEnd.Areas.Admin.Models.AIToolCategoryViewModel
{
    public class AIToolCategoryViewModel
    {
        [Display(Name = "Mã thể loại")]
        public int CategoryID { get; set; }

        [Display(Name = "Tên thể loại")]
        public string CategoryName { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; }
    }
}
