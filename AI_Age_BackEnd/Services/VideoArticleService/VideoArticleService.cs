using AI_Age_BackEnd.DTOs.VideoArticleDTO;
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using Google.Apis.Drive.v3;
using GoogleFile = Google.Apis.Drive.v3.Data.File;
using GooglePermission = Google.Apis.Drive.v3.Data.Permission;

namespace AI_Age_BackEnd.Services.VideoArticleService
{
    public class VideoArticleService
    {
        private readonly IVideoArticleRepository _videoArticleRepository;
        private readonly IVideoArticleCategoryRepository _categoryRepository;
        private readonly DriveService _driveService;

        public VideoArticleService(
            IVideoArticleRepository videoArticleRepository,
            IVideoArticleCategoryRepository categoryRepository,
            DriveService driveService)
        {
            _videoArticleRepository = videoArticleRepository;
            _categoryRepository = categoryRepository;
            _driveService = driveService;
        }

        private async Task<string> UploadFileToGoogleDrive(IFormFile file, string contentType)
        {
            if (file == null || file.Length == 0)
                return null;

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;

            var fileMetadata = new GoogleFile
            {
                Name = file.FileName,
                Parents = new List<string> { "122TaaCDX_cc1w9dezs5PdxBU784z3GVX" }
            };

            var request = _driveService.Files.Create(fileMetadata, stream, contentType);
            request.Fields = "id, webViewLink";

            var uploadedFile = await request.UploadAsync();
            if (uploadedFile.Status != Google.Apis.Upload.UploadStatus.Completed)
            {
                throw new Exception($"Upload failed: {uploadedFile.Exception?.Message ?? "Unknown error"}");
            }

            var uploadedFileMetadata = request.ResponseBody;
            if (uploadedFileMetadata == null || string.IsNullOrEmpty(uploadedFileMetadata.Id))
            {
                throw new Exception("Failed to retrieve file ID after upload.");
            }

            var fileId = uploadedFileMetadata.Id;

            var permission = new GooglePermission
            {
                Type = "anyone",
                Role = "reader"
            };
            await _driveService.Permissions.Create(permission, fileId).ExecuteAsync();

            return $"https://drive.google.com/thumbnail?id={fileId}&sz=w1200";
        }

        public async Task<VideoArticleDto> CreateVideoArticleAsync(VideoArticleCreateDto dto)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(dto.CategoryId);
            if (category == null)
                throw new Exception("Danh mục không tồn tại.");

            var videoUrl = await UploadFileToGoogleDrive(dto.Video, "video/mp4");
            var thumbnailUrl = dto.Thumbnail != null ? await UploadFileToGoogleDrive(dto.Thumbnail, "image/jpeg") : null;

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
                Views = 0
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
                Duration = videoArticle.Duration
            };
        }

        public async Task<List<VideoArticleDto>> GetAllVideoArticlesAsync()
        {
            var videoArticles = await _videoArticleRepository.GetAllVideoArticlesAsync();
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
                Duration = video.Duration
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

            var videoUrl = dto.Video != null ? await UploadFileToGoogleDrive(dto.Video, "video/mp4") : videoArticle.VideoUrl;
            var thumbnailUrl = dto.Thumbnail != null ? await UploadFileToGoogleDrive(dto.Thumbnail, "image/jpeg") : videoArticle.Thumbnail;

            videoArticle.Title = dto.Title;
            videoArticle.Description = dto.Description;
            videoArticle.VideoUrl = videoUrl;
            videoArticle.Thumbnail = thumbnailUrl;
            videoArticle.CategoryId = dto.CategoryId;
            videoArticle.Level = dto.Level;
            videoArticle.Duration = dto.Duration;
            videoArticle.UpdatedDate = DateTime.Now;

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
    }
}
