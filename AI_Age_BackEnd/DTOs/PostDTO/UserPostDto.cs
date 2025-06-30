using AI_Age_BackEnd.DTOs.UserPostCommentDTO;

namespace AI_Age_BackEnd.DTOs.PostDTO
{
    public class UserPostDto
    {
        public int PostID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? AttachedImage { get; set; }
        public DateTime? PostedDate { get; set; }

        public int UserID { get; set; }
        public string UserFullName { get; set; }
        public string? UserAvatar { get; set; }

        public List<UserPostCommentDto> Comments { get; set; }
    }
}
