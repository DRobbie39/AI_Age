﻿using AI_Age_BackEnd.Models;

namespace AI_Age_BackEnd.Repositories.Interfaces
{
    public interface IArticleRepository
    {
        Task<Article> GetArticleByIdAsync(int id);
        Task<List<Article>> GetAllArticlesAsync(string? searchQuery = null, int? categoryId = null);
        Task AddArticleAsync(Article article);
        Task UpdateArticleAsync(Article article);
        Task DeleteArticleAsync(int id);
        Task IncrementViewCountAsync(int id);
        Task<List<Article>> GetByToolIdAsync(int toolId);
    }
}
