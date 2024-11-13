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
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [Route("WebApplication/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFMPService _fmpService;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager, IFMPService fmpService)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
            _fmpService = fmpService;
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

        [HttpPost("{symbol:alpha}")]
        public async Task<IActionResult> Create([FromRoute] string symbol, CreateCommentDTO commentDTO) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if (stock == null) {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null) {
                    return BadRequest("This stock does not exist");
                }
                else {
                    await _stockRepo.CreateAsync(stock);
                }
            }

            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var commentModel = commentDTO.ToCommentFromCreate(stock.ID);
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