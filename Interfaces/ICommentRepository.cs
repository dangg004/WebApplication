using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTOs.Comment;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIDAsync(int ID);
        Task<Comment> CreateAsync(Comment commentModel);
        Task<Comment?> UpdateAsync(int ID, UpdateCommentRequestDTO commentDTO);
        Task<Comment?> DeleteAsync(int ID);
    }
}