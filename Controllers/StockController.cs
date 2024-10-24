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
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _context = context;
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
            var stock = await _context.Stock.FindAsync(ID);
            if (stock == null) {
                return NotFound();
            }
            return Ok(stock.ToStockDTO());
        } 

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO stockDTO) {
            var stockModel = stockDTO.ToStockFromCreateDTO();
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByID), new { ID = stockModel.ID }, stockModel.ToStockDTO());
        }      

        [HttpPut("{ID}")]
        public async Task<IActionResult> Update([FromRoute] int ID, [FromBody] UpdateStockRequestDTO updateDTO) {
            var stockModel = await _context.Stock.FindAsync(ID);

            if (stockModel == null) {
                return NotFound();
            }

            stockModel.Symbol = updateDTO.Symbol;
            stockModel.CompanyName = updateDTO.CompanyName;
            stockModel.Industry = updateDTO.Industry;
            stockModel.LastDiv = updateDTO.LastDiv;
            stockModel.Purchase = updateDTO.Purchase;
            stockModel.MarketCap = updateDTO.MarketCap;

            await _context.SaveChangesAsync();

            return Ok(stockModel.ToStockDTO());
        }

        [HttpDelete("{ID}")]
        public async Task<IActionResult> Delete([FromRoute] int ID) {
            var stockModel = await _context.Stock.FindAsync(ID);

            if (stockModel == null) {
                return NotFound();
            }

            _context.Stock.Remove(stockModel);

            await _context.SaveChangesAsync();

            return NoContent();
        }    
    }
}