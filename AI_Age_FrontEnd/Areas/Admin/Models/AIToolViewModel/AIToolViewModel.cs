using System.ComponentModel.DataAnnotations;

namespace AI_Age_FrontEnd.Areas.Admin.Models.AIToolViewModel
{
    public class AIToolViewModel
    {
        [Display(Name = "Mã công cụ")]
        public int ToolID { get; set; }

        [Display(Name = "Tên công cụ")]
        public string ToolName { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Logo")]
        public string? LogoURL { get; set; }

        [Display(Name = "Website")]
        public string? WebsiteURL { get; set; }

        [Display(Name = "Mã thể loại")]
        public int? CategoryID { get; set; }

        [Display(Name = "Thể loại")]
        public string? CategoryName { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; }
    }
}
