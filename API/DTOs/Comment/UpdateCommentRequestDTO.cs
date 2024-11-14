using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.DTOs.Comment
{
    public class UpdateCommentRequestDTO
    {
        [Required]
        [MinLength(7, ErrorMessage = "Title must be at least 7 characters")]
        [MaxLength(255, ErrorMessage = "Title cannot be over 255 characters")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(7, ErrorMessage = "Content must be at least 7 characters")]
        [MaxLength(255, ErrorMessage = "Content cannot be over 255 characters")]
        public string Content { get; set; } = string.Empty;
    }
}