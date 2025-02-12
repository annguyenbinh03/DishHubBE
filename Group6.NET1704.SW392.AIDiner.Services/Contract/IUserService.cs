using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.Contract
{
    public interface IUserService
    {
        public Task<ResponseDTO> GetAllUser(int pageNumber, int pageSize);
        public Task<ResponseDTO> GetUserById(int id);
        public Task<ResponseDTO> UpdateUserForAdmin(UpdateUserModel userDTO);
    }
}
