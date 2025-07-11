﻿using AI_Age_BackEnd.Models;
using AI_Age_BackEnd.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AI_Age_BackEnd.Repositories
{
    public class VideoArticleRepository : IVideoArticleRepository
    {
        private readonly AI_AgeContext _context;

        public VideoArticleRepository(AI_AgeContext context)
        {
            _context = context;
        }

        public async Task<VideoArticle> GetVideoArticleByIdAsync(int id)
        {
            return await _context.VideoArticles
                .Include(v => v.Category)
                .Include(v => v.UploaderNavigation)
                .Include(a => a.Tool)
                .FirstOrDefaultAsync(v => v.VideoId == id);
        }

        public async Task<List<VideoArticle>> GetAllVideoArticlesAsync(string? searchQuery = null, int? categoryId = null)
        {
            var query = _context.VideoArticles
                .Include(v => v.Category)
                .Include(v => v.UploaderNavigation)
                .Include(a => a.Tool)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                var lowerCaseSearchQuery = searchQuery.ToLower();
                query = query.Where(a =>
                    a.Title.ToLower().Contains(lowerCaseSearchQuery));
            }

            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(v => v.CategoryId == categoryId);
            }

            return await query.OrderByDescending(a => a.PostedDate).ToListAsync();
        }

        public async Task AddVideoArticleAsync(VideoArticle videoArticle)
        {
            await _context.VideoArticles.AddAsync(videoArticle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVideoArticleAsync(VideoArticle videoArticle)
        {
            _context.VideoArticles.Update(videoArticle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVideoArticleAsync(int id)
        {
            var videoArticle = await _context.VideoArticles.FindAsync(id);
            if (videoArticle != null)
            {
                _context.VideoArticles.Remove(videoArticle);
                await _context.SaveChangesAsync();
            }
        }

        public async Task IncrementViewCountAsync(int id)
        {
            var videoArticle = await _context.VideoArticles.FindAsync(id);
            if (videoArticle != null)
            {
                videoArticle.Views = (videoArticle.Views ?? 0) + 1;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<VideoArticle>> GetByToolIdAsync(int toolId)
        {
            return await _context.VideoArticles
                .Where(a => a.ToolId == toolId)
                .OrderByDescending(a => a.PostedDate)
                .ToListAsync();
        }
    }
}
