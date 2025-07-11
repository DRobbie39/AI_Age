﻿using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AI_Age_BackEnd.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly AI_AgeContext _context;

        public ArticleRepository(AI_AgeContext context)
        {
            _context = context;
        }

        public async Task<Article> GetArticleByIdAsync(int id)
        {
            return await _context.Articles
                .Include(a => a.Category)
                .Include(a => a.AuthorNavigation)
                .Include(a => a.Tool)
                .FirstOrDefaultAsync(a => a.ArticleId == id);
        }

        public async Task<List<Article>> GetAllArticlesAsync(string? searchQuery = null, int? categoryId = null)
        {
            var query = _context.Articles
               .Include(a => a.Category)
               .Include(a => a.AuthorNavigation)
               .Include(a => a.Tool)
               .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                var lowerCaseSearchQuery = searchQuery.ToLower();
                query = query.Where(a =>
                    a.Title.ToLower().Contains(lowerCaseSearchQuery) ||
                    a.Summary.ToLower().Contains(lowerCaseSearchQuery));
            }

            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(a => a.CategoryId == categoryId);
            }

            return await query.OrderByDescending(a => a.PostedDate).ToListAsync();
        }

        public async Task AddArticleAsync(Article article)
        {
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateArticleAsync(Article article)
        {
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteArticleAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }

        public async Task IncrementViewCountAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                article.Views = (article.Views ?? 0) + 1;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Article>> GetByToolIdAsync(int toolId)
        {
            return await _context.Articles 
                .Where(a => a.ToolId == toolId)
                .OrderByDescending(a => a.PostedDate)
                .ToListAsync();
        }
    }
}
