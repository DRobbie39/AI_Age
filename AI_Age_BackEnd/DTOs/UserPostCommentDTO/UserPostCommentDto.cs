namespace AI_Age_BackEnd.DTOs.UserPostCommentDTO
{
    public class UserPostCommentDto
    {
        public int CommentID { get; set; }
        public string Content { get; set; }
        public DateTime? CommentDate { get; set; }
        public int UserID { get; set; }
        public string UserFullName { get; set; }
        public string? UserAvatar { get; set; }
    }
}
