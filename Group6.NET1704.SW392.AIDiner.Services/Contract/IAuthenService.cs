using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.DAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.Contract
{
    public interface IAuthenService
    {
        Task<string> Login(LoginDTO model);
        Task<string> Register(RegisterLoginModel model);
        Task<bool> Logout(string userId);
        bool IsTokenRevoked(string token);
    }
}
