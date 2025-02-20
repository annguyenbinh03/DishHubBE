using Group6.NET1704.SW392.AIDiner.Common.Response;
using Group6.NET1704.SW392.AIDiner.DAL.Data;
using Group6.NET1704.SW392.AIDiner.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly DishHub5Context _context;

        public RestaurantRepository(DishHub5Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RestaurantGetAllResponse>> GetAll()
        {
           var response = await _context.Restaurants
           .Select(r => new RestaurantGetAllResponse
           {
               Id = r.Id,
               Name = r.Name,
               Address = r.Address,
               PhoneNumber = r.PhoneNumber,
               Image = r.Image,
               IsDeleted = r.IsDeleted,
               CreatedAt = r.CreatedAt
           })
           .ToListAsync();
            return response;
        }

        public async Task<IEnumerable<RestaurantsWithTablesResponse>> GetAllWithTablesAsync()
        {
            List<RestaurantsWithTablesResponse> response = await _context.Restaurants
            .Where(r => !r.IsDeleted) 
            .Include(r => r.Tables.Where(t => !t.IsDeleted))
            .Select(r => new RestaurantsWithTablesResponse
            {
                Id = r.Id,
                Name = r.Name,
                Image = r.Image,
                Tables = r.Tables.Select(t => new RestaurantTables
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList()
            })
            .ToListAsync();

            return response;
        }
    }
}
