using AI_Age_BackEnd.DTOs.ArticleDTO;
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using Google.Apis.Drive.v3;
using GoogleFile = Google.Apis.Drive.v3.Data.File;
using GooglePermission = Google.Apis.Drive.v3.Data.Permission;

namespace AI_Age_BackEnd.Services.ArticleService
{
    public class ArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleCategoryRepository _categoryRepository;
        private readonly IArticleImageRepository _imageRepository;
        private readonly DriveService _driveService;

        public ArticleService(
            IArticleRepository articleRepository,
            IArticleCategoryRepository categoryRepository,
            IArticleImageRepository imageRepository,
            DriveService driveService)
        {
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _imageRepository = imageRepository;
            _driveService = driveService;
        }

        private async Task<string> UploadImageToGoogleDrive(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return null;

            using var stream = new MemoryStream();
            await image.CopyToAsync(stream);
            stream.Position = 0;

            var fileMetadata = new GoogleFile
            {
                Name = image.FileName,
                Parents = new List<string> { "122TaaCDX_cc1w9dezs5PdxBU784z3GVX" }
            };

            var request = _driveService.Files.Create(fileMetadata, stream, image.ContentType);
            request.Fields = "id, webViewLink";

            var uploadedFile = await request.UploadAsync();

            if (uploadedFile.Status != Google.Apis.Upload.UploadStatus.Completed)
            {
                throw new Exception($"Upload failed: {uploadedFile.Exception?.Message ?? "Unknown error"}");
            }

            var uploadedFileMetadata = request.ResponseBody;
            // File ID is available in fileMetadata after successful upload
            if (uploadedFileMetadata == null || string.IsNullOrEmpty(uploadedFileMetadata.Id))
            {
                throw new Exception("Failed to retrieve file ID after upload.");
            }

            var fileId = uploadedFileMetadata.Id;

            // Set file permissions to public
            var permission = new GooglePermission
            {
                Type = "anyone",
                Role = "reader"
            };
            await _driveService.Permissions.Create(permission, fileId).ExecuteAsync();

            // Retrieve file metadata to get WebViewLink
            var file = await _driveService.Files.Get(fileId).ExecuteAsync();
            return file.WebViewLink;
        }

        public async Task<ArticleDto> CreateArticleAsync(ArticleCreateDto dto)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(dto.CategoryId);
            if (category == null)
                throw new Exception("Danh mục không tồn tại.");

            var imageUrl = await UploadImageToGoogleDrive(dto.Image);

            var article = new Article
            {
                Title = dto.Title,
                Summary = dto.Summary,
                Content = dto.Content,
                Image = imageUrl,
                CategoryId = dto.CategoryId,
                Author = dto.AuthorId,
                PostedDate = DateTime.Now,
                Level = dto.Level,
                Views = 0
            };

            await _articleRepository.AddArticleAsync(article);

            if (dto.AdditionalImages != null && dto.AdditionalImages.Any())
            {
                foreach (var additionalImage in dto.AdditionalImages)
                {
                    var additionalImageUrl = await UploadImageToGoogleDrive(additionalImage);
                    if (additionalImageUrl != null)
                    {
                        await _imageRepository.AddArticleImageAsync(new ArticleImage
                        {
                            ArticleId = article.ArticleId,
                            ImageUrl = additionalImageUrl
                        });
                    }
                }
            }

            return await GetArticleByIdAsync(article.ArticleId);
        }

        public async Task<ArticleDto> GetArticleByIdAsync(int id)
        {
            var article = await _articleRepository.GetArticleByIdAsync(id);
            if (article == null)
                throw new Exception("Bài viết không tồn tại.");

            return new ArticleDto
            {
                ArticleId = article.ArticleId,
                Title = article.Title,
                Summary = article.Summary,
                Content = article.Content,
                Image = article.Image,
                CategoryId = article.CategoryId,
                CategoryName = article.Category?.CategoryName,
                AuthorId = article.Author,
                AuthorName = article.AuthorNavigation?.FullName,
                PostedDate = article.PostedDate,
                UpdatedDate = article.UpdatedDate,
                Views = article.Views,
                Level = article.Level,
                AdditionalImages = article.ArticleImages?.Select(img => img.ImageUrl).ToList()
            };
        }

        public async Task<List<ArticleDto>> GetAllArticlesAsync()
        {
            var articles = await _articleRepository.GetAllArticlesAsync();
            return articles.Select(article => new ArticleDto
            {
                ArticleId = article.ArticleId,
                Title = article.Title,
                Summary = article.Summary,
                Content = article.Content,
                Image = article.Image,
                CategoryId = article.CategoryId,
                CategoryName = article.Category?.CategoryName,
                AuthorId = article.Author,
                AuthorName = article.AuthorNavigation?.FullName,
                PostedDate = article.PostedDate,
                UpdatedDate = article.UpdatedDate,
                Views = article.Views,
                Level = article.Level,
                AdditionalImages = article.ArticleImages?.Select(img => img.ImageUrl).ToList()
            }).ToList();
        }

        public async Task<ArticleDto> UpdateArticleAsync(ArticleUpdateDto dto)
        {
            var article = await _articleRepository.GetArticleByIdAsync(dto.ArticleId);
            if (article == null)
                throw new Exception("Bài viết không tồn tại.");

            var category = await _categoryRepository.GetCategoryByIdAsync(dto.CategoryId);
            if (category == null)
                throw new Exception("Danh mục không tồn tại.");

            var imageUrl = dto.Image != null ? await UploadImageToGoogleDrive(dto.Image) : article.Image;

            article.Title = dto.Title;
            article.Summary = dto.Summary;
            article.Content = dto.Content;
            article.Image = imageUrl;
            article.CategoryId = dto.CategoryId;
            article.Level = dto.Level;
            article.UpdatedDate = DateTime.Now;

            await _articleRepository.UpdateArticleAsync(article);

            if (dto.AdditionalImages != null && dto.AdditionalImages.Any())
            {
                await _imageRepository.DeleteArticleImagesByArticleIdAsync(article.ArticleId);
                foreach (var additionalImage in dto.AdditionalImages)
                {
                    var additionalImageUrl = await UploadImageToGoogleDrive(additionalImage);
                    if (additionalImageUrl != null)
                    {
                        await _imageRepository.AddArticleImageAsync(new ArticleImage
                        {
                            ArticleId = article.ArticleId,
                            ImageUrl = additionalImageUrl
                        });
                    }
                }
            }

            return await GetArticleByIdAsync(article.ArticleId);
        }

        public async Task DeleteArticleAsync(int id)
        {
            var article = await _articleRepository.GetArticleByIdAsync(id);
            if (article == null)
                throw new Exception("Bài viết không tồn tại.");

            await _imageRepository.DeleteArticleImagesByArticleIdAsync(id);
            await _articleRepository.DeleteArticleAsync(id);
        }
    }
}
