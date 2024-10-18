using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.DTOs.Stock;
using WebApplication1.Mappers;

namespace WebApplication1.Controllers
{
    [Route("WebApplication/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll() {
            var stocks = _context.Stock.ToList().Select(s => s.ToStockDTO());
            return Ok(stocks);
        }

        [HttpGet("{ID}")]
        public IActionResult GetByID([FromRoute] int ID) {
            var stock = _context.Stock.Find(ID);
            if (stock == null) {
                return NotFound();
            }
            return Ok(stock.ToStockDTO());
        } 

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDTO stockDTO) {
            var stockModel = stockDTO.ToStockFromCreateDTO();
            _context.Stock.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetByID), new { ID = stockModel.ID }, stockModel.ToStockDTO());
        }      
    }
}