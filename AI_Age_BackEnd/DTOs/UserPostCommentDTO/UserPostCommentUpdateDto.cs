using System.ComponentModel.DataAnnotations;

namespace AI_Age_BackEnd.DTOs.UserPostCommentDTO
{
    public class UserPostCommentUpdateDto
    {
        [Required(ErrorMessage = "Nội dung bình luận không được để trống.")]
        public string Content { get; set; }
    }
}
