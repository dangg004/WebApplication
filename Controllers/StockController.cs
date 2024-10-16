using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    [Route("WebApplication1/stock")]
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
            var stocks = _context.Stock.ToList();
            return Ok(stocks);
        }

        [HttpGet("{ID}")]
        public IActionResult GetByID([FromRoute] int ID){
            var stock = _context.Stock.Find(ID);
            if (stock == null) {
                return NotFound();
            }
            return Ok(stock);
        }       
    }
}