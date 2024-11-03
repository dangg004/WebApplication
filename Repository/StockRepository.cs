using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs.Stock;
using WebApplication1.Helpers;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context) 
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int ID)
        {
            var stockModel = await _context.Stock.FindAsync(ID);

            if (stockModel == null) {
                return null;
            }

            _context.Stock.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stock = _context.Stock.Include(c => c.Comments).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName)) {
                stock = stock.Where(c => c.CompanyName.Contains(query.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol)) {
                stock = stock.Where(c => c.Symbol.Contains(query.Symbol));
            }
            if (!string.IsNullOrWhiteSpace(query.SortBy)) {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase)) {
                    stock = query.IsDescending ? stock.OrderByDescending(s => s.Symbol) : stock.OrderBy(s => s.Symbol); 
                }
            }
            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            return await stock.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIDAsync(int ID)
        {
            return await _context.Stock.Include(c => c.Comments).FirstOrDefaultAsync(i => i.ID == ID);
        }

        public async Task<bool> StockExists(int ID)
        {
            return await _context.Stock.AnyAsync(s => s.ID == ID);
        }

        public async Task<Stock?> UpdateAsync(int ID, UpdateStockRequestDTO updateDTO)
        {
            var stockModel = await _context.Stock.FindAsync(ID);

            if (stockModel == null) {
                return null;
            }

            stockModel.Symbol = updateDTO.Symbol;
            stockModel.CompanyName = updateDTO.CompanyName;
            stockModel.Industry = updateDTO.Industry;
            stockModel.LastDiv = updateDTO.LastDiv;
            stockModel.Purchase = updateDTO.Purchase;
            stockModel.MarketCap = updateDTO.MarketCap;

            await _context.SaveChangesAsync();
            return stockModel;
        }
    }
}