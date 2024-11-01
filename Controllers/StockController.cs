using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs.Stock;
using WebApplication1.Interfaces;
using WebApplication1.Mappers;

namespace WebApplication1.Controllers
{
    [Route("WebApplication/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var stocks = await _stockRepo.GetAllAsync();
            var stockDTO = stocks.Select(s => s.ToStockDTO());
            return Ok(stockDTO);
        }

        [HttpGet("{ID}")]
        public async Task<IActionResult> GetByID([FromRoute] int ID) {
            var stock = await _stockRepo.GetByIDAsync(ID);
            if (stock == null) {
                return NotFound();
            }
            return Ok(stock.ToStockDTO());
        } 

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO stockDTO) {
            var stockModel = stockDTO.ToStockFromCreateDTO();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetByID), new { ID = stockModel.ID }, stockModel.ToStockDTO());
        }      

        [HttpPut("{ID}")]
        public async Task<IActionResult> Update([FromRoute] int ID, [FromBody] UpdateStockRequestDTO updateDTO) {
            var stockModel = await _stockRepo.UpdateAsync(ID, updateDTO);

            if (stockModel == null) {
                return NotFound();
            }

            return Ok(stockModel.ToStockDTO());
        }

        [HttpDelete("{ID}")]
        public async Task<IActionResult> Delete([FromRoute] int ID) {
            var stockModel = await _stockRepo.DeleteAsync(ID);

            if (stockModel == null) {
                return NotFound();
            }

            return NoContent();
        }    
    }
}