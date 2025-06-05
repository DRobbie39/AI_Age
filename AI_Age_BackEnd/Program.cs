
using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using AI_Age_BackEnd.Repositories;
using Microsoft.EntityFrameworkCore;
using AI_Age_BackEnd.Services.UserService;
using AI_Age_BackEnd.Services.ArticleService;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

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
            builder.Services.AddScoped<IArticleImageRepository, ArticleImageRepository>();

            // Đăng ký các service
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<ArticleService>();

            // Đăng ký Google Drive Service
            builder.Services.AddSingleton(provider =>
            {
                var credential = GoogleCredential.FromFile("Credentials/ai-age-462018-11f0c4a35baa.json")
                    .CreateScoped(DriveService.Scope.Drive);
                return new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "AI Age Support"
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
