using AI_Age_BackEnd.DTOs.ArticleDTO;
using AI_Age_BackEnd.DTOs.RatingDTO;
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace AI_Age_BackEnd.Services.ArticleService
{
    public class ArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleCategoryRepository _categoryRepository;
        private readonly IArticleRatingRepository _ratingRepository;
        private readonly Cloudinary _cloudinary;

        public ArticleService(
            IArticleRepository articleRepository,
            IArticleCategoryRepository categoryRepository,
            IArticleRatingRepository ratingRepository,
            Cloudinary cloudinary)
        {
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _ratingRepository = ratingRepository;
            _cloudinary = cloudinary;
        }

        private async Task<string> UploadImageToCloudinary(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return null;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(image.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                throw new Exception("Định dạng hình ảnh không hợp lệ. Chỉ hỗ trợ JPG, PNG.");

            using var stream = image.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, stream),
                Transformation = new Transformation().Width(1200).Crop("fit")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception($"Upload failed: {uploadResult.Error?.Message ?? "Unknown error"}");

            return uploadResult.SecureUrl.ToString();
        }

        public async Task<ArticleDto> CreateArticleAsync(ArticleCreateDto dto)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(dto.CategoryId);
            if (category == null)
                throw new Exception("Danh mục không tồn tại.");

            var imageUrl = await UploadImageToCloudinary(dto.Image);

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
                Views = 0,
                AverageRating = 0.0m
            };

            await _articleRepository.AddArticleAsync(article);

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
                AverageRating = article.AverageRating ?? 0.0m
            };
        }

        public async Task<List<ArticleDto>> GetAllArticlesAsync(string? searchQuery = null)
        {
            var articles = await _articleRepository.GetAllArticlesAsync(searchQuery);
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
                AverageRating = article.AverageRating ?? 0.0m
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

            var imageUrl = dto.Image != null ? await UploadImageToCloudinary(dto.Image) : article.Image;

            article.Title = dto.Title;
            article.Summary = dto.Summary;
            article.Content = dto.Content;
            article.Image = imageUrl;
            article.CategoryId = dto.CategoryId;
            article.Level = dto.Level;
            article.UpdatedDate = DateTime.Now;

            await _articleRepository.UpdateArticleAsync(article);

            return await GetArticleByIdAsync(article.ArticleId);
        }

        public async Task DeleteArticleAsync(int id)
        {
            var article = await _articleRepository.GetArticleByIdAsync(id);
            if (article == null)
                throw new Exception("Bài viết không tồn tại.");

            await _articleRepository.DeleteArticleAsync(id);
        }

        public async Task AddRatingAsync(ArticleRatingCreateDto dto)
        {
            if (dto.RatingValue < 1 || dto.RatingValue > 5)
                throw new Exception("Rating must be between 1 and 5.");

            var article = await _articleRepository.GetArticleByIdAsync(dto.ArticleId);
            if (article == null)
                throw new Exception("Bài viết không tồn tại.");

            var rating = new ArticleRating
            {
                ArticleId = dto.ArticleId,
                UserId = dto.UserId,
                RatingValue = dto.RatingValue,
                CreatedDate = DateTime.Now
            };

            await _ratingRepository.AddRatingAsync(rating);

            article.AverageRating = await _ratingRepository.GetAverageRatingAsync(dto.ArticleId);
            await _articleRepository.UpdateArticleAsync(article);
        }

        public async Task<int?> GetUserRatingAsync(int articleId, int userId)
        {
            var rating = await _ratingRepository.GetUserRatingAsync(articleId, userId);
            return rating?.RatingValue;
        }

        public async Task IncrementViewCountAsync(int id)
        {
            await _articleRepository.IncrementViewCountAsync(id);
        }

    }
}
