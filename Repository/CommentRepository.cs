using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs.Comment;
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

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIDAsync(int ID)
        {
            return await _context.Comments.FindAsync(ID);
        }

        public async Task<Comment?> UpdateAsync(int ID, UpdateCommentRequestDTO commentDTO)
        {
            var existingComment = await _context.Comments.FindAsync(ID);
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