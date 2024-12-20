using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTOs.Comment;
using WebApplication1.Models;

namespace WebApplication1.Mappers
{
    public static class CommentMappers
    {
        public static CommentDTO ToCommentDTO(this Comment commentModel) {//+
            return new CommentDTO {
                ID = commentModel.ID,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                CreatedBy = commentModel.AppUser.UserName,
                StockID = commentModel.StockID
            };
        }

        public static Comment ToCommentFromCreate(this CreateCommentDTO commentDTO, int stockID) {
            return new Comment {
                Title = commentDTO.Title,
                Content = commentDTO.Content,
                StockID = stockID,
            };
        }

    }
}    