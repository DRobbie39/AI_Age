
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using AI_Age_BackEnd.Repositories;
using Microsoft.EntityFrameworkCore;
using AI_Age_BackEnd.Services.UserService;
using AI_Age_BackEnd.Services.ArticleService;
using AI_Age_BackEnd.Services.ChatService;
using AI_Age_BackEnd.Services.VideoArticleService;
using AI_Age_BackEnd.Services.AIToolService;
using AI_Age_BackEnd.Services.AIToolCategoryService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CloudinaryDotNet;
using AI_Age_BackEnd.Services.UserPostService;
using AI_Age_BackEnd.Services.UserPostCommentService;
using AI_Age_BackEnd.Services.SavedLessonService;

namespace AI_Age_BackEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Đăng ký database
            builder.Services.AddDbContext<AI_AgeContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Đăng ký các repository
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
            builder.Services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
            builder.Services.AddScoped<IArticleRatingRepository, ArticleRatingRepository>();
            builder.Services.AddScoped<IVideoArticleRepository, VideoArticleRepository>();
            builder.Services.AddScoped<IVideoArticleCategoryRepository, VideoArticleCategoryRepository>();
            builder.Services.AddScoped<IVideoArticleRatingRepository, VideoArticleRatingRepository>();
            builder.Services.AddScoped<IAIToolCategoryRepository, AIToolCategoryRepository>();
            builder.Services.AddScoped<IAIToolRepository, AIToolRepository>();
            builder.Services.AddScoped<IChatHistoryRepository, ChatHistoryRepository>();
            builder.Services.AddScoped<ISearchRepository, SearchRepository>();
            builder.Services.AddScoped<IUserPostRepository, UserPostRepository>();
            builder.Services.AddScoped<IUserPostCommentRepository, UserPostCommentRepository>();
            builder.Services.AddScoped<ISavedLessonRepository, SavedLessonRepository>();

            // Đăng ký các service
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<ArticleService>();
            builder.Services.AddScoped<VideoArticleService>();
            builder.Services.AddScoped<ChatService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<AIToolCategoryService>();
            builder.Services.AddScoped<AIToolService>();
            builder.Services.AddScoped<ChatService>();
            builder.Services.AddScoped<UserPostService>();
            builder.Services.AddScoped<UserPostCommentService, UserPostCommentService>();
            builder.Services.AddScoped<SavedLessonService>();

            // Cấu hình CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Cấu hình JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            // Đăng ký Cloudinary
            builder.Services.AddSingleton(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var cloudName = configuration["Cloudinary:CloudName"];
                var apiKey = configuration["Cloudinary:ApiKey"];
                var apiSecret = configuration["Cloudinary:ApiSecret"];
                var account = new Account(cloudName, apiKey, apiSecret);
                return new Cloudinary(account);
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Cho phép CORS
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
