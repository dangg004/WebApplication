using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.DTOs.Stock;
using WebApplication1.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIDAsync(int ID);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int ID, UpdateStockRequestDTO stockDTO);
        Task<Stock?> DeleteAsync(int ID);
        Task<bool> StockExists(int ID);
    }
}