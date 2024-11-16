using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs.Comment;
using WebApplication1.Helpers;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int ID)
        {
            var existingComment = await _context.Comments.FindAsync(ID);
            if (existingComment == null) {
                return null;
            }
            _context.Comments.Remove(existingComment);
            await _context.SaveChangesAsync();
            return existingComment;
        }

        public async Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject)
        {
            var comments = _context.Comments.Include(a => a.AppUser).AsQueryable();
            if (!string.IsNullOrWhiteSpace(queryObject.Symbol)) {
                comments = comments.Where(s => s.Stock.Symbol == queryObject.Symbol);
            }
            if (queryObject.IsDescending) {
                comments = comments.OrderByDescending(c => c.CreatedOn);
            }
            return await comments.ToListAsync();
        }

        public async Task<Comment?> GetByIDAsync(int ID)
        {
            return await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(a => a.ID == ID);
        }

        public async Task<Comment?> UpdateAsync(int ID, UpdateCommentRequestDTO commentDTO)
        {
            var existingComment = await _context.Comments.Include(a => a.AppUser).FirstOrDefaultAsync(i => i.ID == ID);
            if (existingComment == null) {
                return null;
            }
            existingComment.Title = commentDTO.Title;
            existingComment.Content = commentDTO.Content;
            await _context.SaveChangesAsync();
            return existingComment;
        }
    }
}