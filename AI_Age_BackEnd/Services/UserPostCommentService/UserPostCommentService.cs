using AI_Age_BackEnd.DTOs.UserPostCommentDTO;
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;

namespace AI_Age_BackEnd.Services.UserPostCommentService
{
    public class UserPostCommentService
    {
        private readonly IUserPostCommentRepository _commentRepository;
        private readonly IUserPostRepository _postRepository;

        public UserPostCommentService(IUserPostCommentRepository commentRepository, IUserPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }

        private UserPostCommentDto MapToCommentDto(UserPostComment comment)
        {
            return new UserPostCommentDto
            {
                CommentID = comment.CommentId,
                Content = comment.Content,
                CommentDate = comment.CommentDate,
                UserID = comment.UserId,
                UserFullName = comment.User?.FullName ?? "Người dùng ẩn danh",
                UserAvatar = comment.User?.Avatar
            };
        }

        public async Task<UserPostComment> GetCommentByIdAsync(int id)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null)
            {
                throw new KeyNotFoundException("Không tìm thấy bình luận.");
            }
            return comment;
        }

        public async Task<UserPostCommentDto> CreateCommentAsync(int postId, UserPostCommentCreateDto createDto, int userId)
        {
            // Kiểm tra xem bài viết có tồn tại không
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post == null)
            {
                throw new KeyNotFoundException("Không tìm thấy bài viết để bình luận.");
            }

            var comment = new UserPostComment
            {
                Content = createDto.Content,
                PostId = postId,
                UserId = userId,
                CommentDate = DateTime.UtcNow
            };

            var newComment = await _commentRepository.AddCommentAsync(comment);

            // Lấy lại comment cùng với thông tin User để trả về DTO đầy đủ
            var result = await _commentRepository.GetCommentByIdAsync(newComment.CommentId);
            return MapToCommentDto(result!);
        }

        public async Task<UserPostCommentDto> UpdateCommentAsync(int commentId, UserPostCommentUpdateDto updateDto, int userId)
        {
            var existingComment = await _commentRepository.GetCommentByIdAsync(commentId);
            if (existingComment == null)
            {
                throw new KeyNotFoundException("Không tìm thấy bình luận.");
            }

            if (existingComment.UserId != userId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền sửa bình luận này.");
            }

            existingComment.Content = updateDto.Content;
            await _commentRepository.UpdateCommentAsync(existingComment);

            return MapToCommentDto(existingComment);
        }

        public async Task DeleteCommentAsync(int commentId, int userId)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);
            if (comment == null)
            {
                throw new KeyNotFoundException("Không tìm thấy bình luận.");
            }

            if (comment.UserId != userId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa bình luận này.");
            }

            await _commentRepository.DeleteCommentAsync(commentId);
        }
    }
}
