using AI_Age_BackEnd.DTOs.PostDTO;
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using AI_Age_BackEnd.DTOs.UserPostCommentDTO;

namespace AI_Age_BackEnd.Services.UserPostService
{
    public class UserPostService
    {
        private readonly IUserPostRepository _postRepository;
        private readonly Cloudinary _cloudinary;

        public UserPostService(IUserPostRepository postRepository, Cloudinary cloudinary)
        {
            _postRepository = postRepository;
            _cloudinary = cloudinary;
        }

        private async Task<string?> UploadImageToCloudinaryAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            var uploadResult = new ImageUploadResult();
            using (var stream = imageFile.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageFile.FileName, stream),
                    Folder = "user_posts_images"
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            if (uploadResult.Error != null)
            {
                throw new Exception($"Lỗi tải ảnh lên Cloudinary: {uploadResult.Error.Message}");
            }

            return uploadResult.SecureUrl.ToString();
        }

        public async Task<IEnumerable<UserPostDto>> GetAllPostsAsync()
        {
            var posts = await _postRepository.GetAllPostsAsync();

            return posts.Select(post => new UserPostDto
            {
                PostID = post.PostId,
                Title = post.Title,
                Content = post.Content,
                AttachedImage = post.AttachedImage,
                PostedDate = post.PostedDate,
                UserID = post.UserId,
                UserFullName = post.User?.FullName ?? "Người dùng ẩn danh",
                UserAvatar = post.User?.Avatar,

                Comments = post.UserPostComments?.Select(c => new UserPostCommentDto
                {
                    CommentID = c.CommentId,
                    Content = c.Content,
                    CommentDate = c.CommentDate,
                    UserID = c.UserId,
                    UserFullName = c.User?.FullName ?? "Người dùng ẩn danh",
                    UserAvatar = c.User?.Avatar
                }).ToList() ?? new List<UserPostCommentDto>()
            });
        }

        public async Task<UserPostDto> GetPostByIdAsync(int id)
        {
            var post = await _postRepository.GetPostByIdAsync(id);
            if (post == null)
            {
                throw new KeyNotFoundException("Không tìm thấy bài viết.");
            }

            return new UserPostDto
            {
                PostID = post.PostId,
                Title = post.Title,
                Content = post.Content,
                AttachedImage = post.AttachedImage,
                PostedDate = post.PostedDate,
                UserID = post.UserId,
                UserFullName = post.User?.FullName ?? "Người dùng ẩn danh",
                UserAvatar = post.User?.Avatar,
                Comments = post.UserPostComments?.Select(c => new UserPostCommentDto
                {
                    CommentID = c.CommentId,
                    Content = c.Content,
                    CommentDate = c.CommentDate,
                    UserID = c.UserId,
                    UserFullName = c.User?.FullName ?? "Người dùng ẩn danh",
                    UserAvatar = c.User?.Avatar
                }).ToList() ?? new List<UserPostCommentDto>()
            };
        }

        public async Task<UserPostDto> CreatePostAsync(UserPostCreateDto createDto, int userId)
        {
            var imageUrl = await UploadImageToCloudinaryAsync(createDto.AttachedImageFile);

            var post = new UserPost
            {
                Title = createDto.Title,
                Content = createDto.Content,
                AttachedImage = imageUrl,
                UserId = userId,
                PostedDate = DateTime.UtcNow
            };

            var newPost = await _postRepository.AddPostAsync(post);

            return await GetPostByIdAsync(newPost.PostId);
        }

        public async Task<UserPostDto> UpdatePostAsync(int postId, UserPostUpdateDto updateDto, int userId)
        {
            var existingPost = await _postRepository.GetPostByIdAsync(postId);
            if (existingPost == null)
            {
                throw new KeyNotFoundException("Không tìm thấy bài viết.");
            }

            if (existingPost.UserId != userId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền sửa bài viết này.");
            }

            if (updateDto.AttachedImageFile != null)
            {
                existingPost.AttachedImage = await UploadImageToCloudinaryAsync(updateDto.AttachedImageFile);
            }

            existingPost.Title = updateDto.Title;
            existingPost.Content = updateDto.Content;

            await _postRepository.UpdatePostAsync(existingPost);

            return await GetPostByIdAsync(postId);
        }

        public async Task DeletePostAsync(int postId, int userId)
        {
            var post = await _postRepository.GetPostByIdAsync(postId);
            if (post == null)
            {
                throw new KeyNotFoundException("Không tìm thấy bài viết.");
            }

            if (post.UserId != userId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa bài viết này.");
            }

            await _postRepository.DeletePostAsync(postId);
        }
    }
}
