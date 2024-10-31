using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Mappers;

namespace WebApplication1.Controllers
{
    [Route("WebApplication/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        public CommentController(ICommentRepository commentRepo)
        {
            _commentRepo = commentRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var comments = await _commentRepo.GetAllAsync();
            var commentDTO = comments.Select(x => x.ToCommentDTO());
            return Ok(commentDTO);
        }

        [HttpGet("{ID}")]
        public async Task<IActionResult> GetByID([FromRoute] int ID) {
            var comments = await _commentRepo.GetByIDAsync(ID);
            if (comments == null) {
                return NotFound();
            }
            return Ok(comments.ToCommentDTO());
        }
    }
}