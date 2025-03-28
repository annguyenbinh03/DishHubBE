using Group6.NET1704.SW392.AIDiner.Common.Request;
using Group6.NET1704.SW392.AIDiner.Common.Response;
using Group6.NET1704.SW392.AIDiner.DAL.Data;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
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
                    Name = t.Name,
                    Status = t.Status
                }).ToList()
            })
            .ToListAsync();

            return response;
        }

        public async Task<RestaurantCreationReponse> Create(RestaurantCreationRequest request)
        {
            var restaurant = new Restaurant
            {
                Name = request.Name,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                Image = request.Image,
                IsDeleted = false, 
                CreatedAt = DateTime.UtcNow 
            };

            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();

            return new RestaurantCreationReponse
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Address = restaurant.Address,
                PhoneNumber = restaurant.PhoneNumber,
                Image = restaurant.Image,
                IsDeleted = restaurant.IsDeleted,
                CreatedAt = restaurant.CreatedAt
            };
        }

        public async Task<RestaurantUpdateReponse> Update(int Id, RestaurantUpdateRequest request)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == Id);
            if (restaurant == null)
            {
                throw new KeyNotFoundException($"Restaurant with ID {Id} not found.");
            }
            restaurant.Name = request.Name;
            restaurant.Address = request.Address;
            restaurant.PhoneNumber = request.PhoneNumber;
            restaurant.Image = request.Image;
            restaurant.IsDeleted = request.IsDeleted;

            // Lưu thay đổi vào database
            await _context.SaveChangesAsync();

            // Trả về response
            return new RestaurantUpdateReponse
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Address = restaurant.Address,
                PhoneNumber = restaurant.PhoneNumber,
                Image = restaurant.Image,
                IsDeleted = restaurant.IsDeleted,
                CreatedAt = restaurant.CreatedAt
            };
        }
    }
}
