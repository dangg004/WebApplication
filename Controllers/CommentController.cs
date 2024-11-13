using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs.Comment;
using WebApplication1.Extensions;
using WebApplication1.Interfaces;
using WebApplication1.Mappers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("WebApplication/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var comments = await _commentRepo.GetAllAsync();
            var commentDTO = comments.Select(x => x.ToCommentDTO());
            return Ok(commentDTO);
        }

        [HttpGet("{ID:int}")]
        public async Task<IActionResult> GetByID([FromRoute] int ID) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var comments = await _commentRepo.GetByIDAsync(ID);
            if (comments == null) {
                return NotFound();
            }
            return Ok(comments.ToCommentDTO());
        }

        [HttpPost("{stockID:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockID, CreateCommentDTO commentDTO) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (!await _stockRepo.StockExists(stockID)) {
                return BadRequest("Stock does not exist");
            }
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var commentModel = commentDTO.ToCommentFromCreate(stockID);
            commentModel.AppUserID = appUser.Id;
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetByID), new { ID = commentModel.ID }, commentModel.ToCommentDTO());
        }

        [HttpPut("{ID:int}")]
        public async Task<IActionResult> Update([FromRoute] int ID, [FromBody] UpdateCommentRequestDTO updateDTO) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var comment = await _commentRepo.UpdateAsync(ID, updateDTO);
            if (comment == null) {
                return NotFound("Comment not found");
            }
            return Ok(comment.ToCommentDTO());
        }

        [HttpDelete("{ID:int}")]
        public async Task<IActionResult> Delete([FromRoute] int ID) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var comment = await _commentRepo.DeleteAsync(ID);
            if (comment == null) {
                return NotFound("Comment not found");
            }
            return NoContent();
        }
    }
}