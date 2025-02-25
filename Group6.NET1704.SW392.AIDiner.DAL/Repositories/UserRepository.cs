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
    public class UserRepository : IUserRepository
    {
        private readonly DishHub5Context _context;

        public UserRepository(DishHub5Context context)
        {
            _context = context;
        }

        public async Task<User?> FindUserByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User> CreateUserAsync(string email, string name)
        {

            //if (await _context.Users.Find(u => u.Username == model.Username))
            //    return "User already exists";

            //if (await _userRepository.ExistsAsync(u => u.Email == model.Email))
            //    return "Email already exists";

            //if (await _userRepository.ExistsAsync(u => u.PhoneNumber == model.PhoneNumber))
            //    return "c";

            var newUser = new User
            {
                Username = email,
                FullName = name,
                Email = email,

                //Dob = model.Dob,
                //PhoneNumber = model.PhoneNumber,
                RoleId = 1,
                CreateAt = DateTime.UtcNow,
                //Address = model.Address,
                //Status = true,
                //Avatar = model.Avatar
            };


            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }
    }
}