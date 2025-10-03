using AI_Age_BackEnd.DTOs.VideoArticleDTO;
using AI_Age_BackEnd.DTOs.VideoArticleRatingDTO;
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories;
using AI_Age_BackEnd.Repositories.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace AI_Age_BackEnd.Services.VideoArticleService
{
    public class VideoArticleService
    {
        private readonly IVideoArticleRepository _videoArticleRepository;
        private readonly IVideoArticleCategoryRepository _categoryRepository;
        private readonly IVideoArticleRatingRepository _ratingRepository;
        private readonly Cloudinary _cloudinary;

        public VideoArticleService(
            IVideoArticleRepository videoArticleRepository,
            IVideoArticleCategoryRepository categoryRepository,
            IVideoArticleRatingRepository ratingRepository,
            Cloudinary cloudinary)
        {
            _videoArticleRepository = videoArticleRepository;
            _categoryRepository = categoryRepository;
            _ratingRepository = ratingRepository;
            _cloudinary = cloudinary;
        }

        private async Task<string> UploadFileToCloudinary(IFormFile file, string contentType)
        {
            if (file == null || file.Length == 0)
                return null;

            var allowedVideoExtensions = new[] { ".mp4", ".mov", ".avi" };
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (contentType.StartsWith("video") && !allowedVideoExtensions.Contains(extension))
                throw new Exception("Định dạng video không hợp lệ. Chỉ hỗ trợ MP4, MOV, AVI.");
            if (contentType.StartsWith("image") && !allowedImageExtensions.Contains(extension))
                throw new Exception("Định dạng hình ảnh không hợp lệ. Chỉ hỗ trợ JPG, PNG.");

            using var stream = file.OpenReadStream();

            if (contentType.StartsWith("video"))
            {
                var uploadParams = new VideoUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Width(1280).Quality(75).Crop("fit")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception($"Upload failed: {uploadResult.Error?.Message ?? "Unknown error"}");

                return uploadResult.SecureUrl.ToString();
            }
            else
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Width(800).Quality("auto").Crop("fit")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception($"Upload failed: {uploadResult.Error?.Message ?? "Unknown error"}");

                return uploadResult.SecureUrl.ToString();
            }
        }

        public async Task<VideoArticleDto> CreateVideoArticleAsync(VideoArticleCreateDto dto)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(dto.CategoryId);
            if (category == null)
                throw new Exception("Danh mục không tồn tại.");

            var videoUrl = await UploadFileToCloudinary(dto.Video, "video/mp4");
            var thumbnailUrl = dto.Thumbnail != null ? await UploadFileToCloudinary(dto.Thumbnail, "image/jpeg") : null;

            var videoArticle = new VideoArticle
            {
                Title = dto.Title,
                Description = dto.Description,
                VideoUrl = videoUrl,
                Thumbnail = thumbnailUrl,
                CategoryId = dto.CategoryId,
                Uploader = dto.UploaderId,
                PostedDate = DateTime.Now,
                Level = dto.Level,
                Duration = dto.Duration,
                Views = 0,
                AverageRating = 0.0m,
                ToolId = dto.ToolID
            };

            await _videoArticleRepository.AddVideoArticleAsync(videoArticle);

            return await GetVideoArticleByIdAsync(videoArticle.VideoId);
        }

        public async Task<VideoArticleDto> GetVideoArticleByIdAsync(int id)
        {
            var videoArticle = await _videoArticleRepository.GetVideoArticleByIdAsync(id);
            if (videoArticle == null)
                throw new Exception("Video không tồn tại.");

            return new VideoArticleDto
            {
                VideoId = videoArticle.VideoId,
                Title = videoArticle.Title,
                Description = videoArticle.Description,
                VideoUrl = videoArticle.VideoUrl,
                Thumbnail = videoArticle.Thumbnail,
                CategoryId = videoArticle.CategoryId,
                CategoryName = videoArticle.Category?.CategoryName,
                UploaderId = videoArticle.Uploader,
                UploaderName = videoArticle.UploaderNavigation?.FullName,
                PostedDate = videoArticle.PostedDate,
                UpdatedDate = videoArticle.UpdatedDate,
                Views = videoArticle.Views,
                Level = videoArticle.Level,
                Duration = videoArticle.Duration,
                AverageRating = videoArticle.AverageRating ?? 0.0m,
                ToolId = videoArticle.ToolId,
                ToolName = videoArticle.Tool?.ToolName
            };
        }

        public async Task<List<VideoArticleDto>> GetAllVideoArticlesAsync(string? searchQuery = null, int? categoryId = null)
        {
            var videoArticles = await _videoArticleRepository.GetAllVideoArticlesAsync(searchQuery, categoryId);
            return videoArticles.Select(video => new VideoArticleDto
            {
                VideoId = video.VideoId,
                Title = video.Title,
                Description = video.Description,
                VideoUrl = video.VideoUrl,
                Thumbnail = video.Thumbnail,
                CategoryId = video.CategoryId,
                CategoryName = video.Category?.CategoryName,
                UploaderId = video.Uploader,
                UploaderName = video.UploaderNavigation?.FullName,
                PostedDate = video.PostedDate,
                UpdatedDate = video.UpdatedDate,
                Views = video.Views,
                Level = video.Level,
                Duration = video.Duration,
                AverageRating = video.AverageRating ?? 0.0m,
                ToolId = video.ToolId,
                ToolName = video.Tool?.ToolName
            }).ToList();
        }

        public async Task<VideoArticleDto> UpdateVideoArticleAsync(VideoArticleUpdateDto dto)
        {
            var videoArticle = await _videoArticleRepository.GetVideoArticleByIdAsync(dto.VideoId);
            if (videoArticle == null)
                throw new Exception("Video không tồn tại.");

            var category = await _categoryRepository.GetCategoryByIdAsync(dto.CategoryId);
            if (category == null)
                throw new Exception("Danh mục không tồn tại.");

            var videoUrl = dto.Video != null ? await UploadFileToCloudinary(dto.Video, "video/mp4") : videoArticle.VideoUrl;
            var thumbnailUrl = dto.Thumbnail != null ? await UploadFileToCloudinary(dto.Thumbnail, "image/jpeg") : videoArticle.Thumbnail;

            videoArticle.Title = dto.Title;
            videoArticle.Description = dto.Description;
            videoArticle.VideoUrl = videoUrl;
            videoArticle.Thumbnail = thumbnailUrl;
            videoArticle.CategoryId = dto.CategoryId;
            videoArticle.Level = dto.Level;
            videoArticle.Duration = dto.Duration;
            videoArticle.UpdatedDate = DateTime.Now;
            videoArticle.ToolId = dto.ToolID;

            await _videoArticleRepository.UpdateVideoArticleAsync(videoArticle);

            return await GetVideoArticleByIdAsync(videoArticle.VideoId);
        }

        public async Task DeleteVideoArticleAsync(int id)
        {
            var videoArticle = await _videoArticleRepository.GetVideoArticleByIdAsync(id);
            if (videoArticle == null)
                throw new Exception("Video không tồn tại.");

            await _videoArticleRepository.DeleteVideoArticleAsync(id);
        }

        public async Task AddRatingAsync(VideoArticleRatingCreateDto dto)
        {
            if (dto.RatingValue < 1 || dto.RatingValue > 5)
                throw new Exception("Rating must be between 1 and 5.");

            var video = await _videoArticleRepository.GetVideoArticleByIdAsync(dto.VideoId);
            if (video == null)
                throw new Exception("Video không tồn tại.");

            var rating = new VideoArticleRating
            {
                VideoId = dto.VideoId,
                UserId = dto.UserId,
                RatingValue = dto.RatingValue,
                CreatedDate = DateTime.Now
            };

            await _ratingRepository.AddRatingAsync(rating);

            video.AverageRating = await _ratingRepository.GetAverageRatingAsync(dto.VideoId);
            await _videoArticleRepository.UpdateVideoArticleAsync(video);
        }

        public async Task<int?> GetUserRatingAsync(int videoId, int userId)
        {
            var rating = await _ratingRepository.GetUserRatingAsync(videoId, userId);
            return rating?.RatingValue;
        }

        public async Task IncrementViewCountAsync(int id)
        {
            await _videoArticleRepository.IncrementViewCountAsync(id);
        }

        public async Task<List<VideoArticleDto>> GetVideoArticlesByToolIdAsync(int toolId)
        {
            var videos = await _videoArticleRepository.GetByToolIdAsync(toolId);

            return videos.Select(video => new VideoArticleDto
            {
                VideoId = video.VideoId,
                Title = video.Title,
                Thumbnail = video.Thumbnail,
                VideoUrl = video.VideoUrl,
                Duration = video.Duration
            }).ToList();
        }
    }
}
