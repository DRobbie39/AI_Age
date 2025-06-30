using System.ComponentModel.DataAnnotations;

namespace AI_Age_BackEnd.DTOs.PostDTO
{
    public class UserPostUpdateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile? AttachedImageFile { get; set; }
    }
}
