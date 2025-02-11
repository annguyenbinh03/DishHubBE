using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.Implementation
{
    public class UserService : IUserService
    {
        private IGenericRepository<User> _userRepository;
        private IUnitOfWork _unitOfWork;

        public UserService(IGenericRepository<User> userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> GetAllUser(int pageNumber, int pageSize)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var users = await _userRepository.GetAllDataByExpression(null, pageNumber: pageNumber, pageSize: pageSize, a => a.Role);

                dto.Data = users.Items.Select(u => new UserDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    FullName = u.FullName,
                    Email = u.Email,
                    Dob = u.Dob,
                    PhoneNumber = u.PhoneNumber,
                    RoleId = u.RoleId,
                    CreateAt = u.CreateAt,
                    Address = u.Address,
                    Status = u.Status,
                    Avatar = u.Avatar

                }).ToList();

                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
            }
            return dto;
        }
        public async Task<ResponseDTO> GetUserById(int id)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                var user = await _userRepository.GetById(id);

                if (user == null)
                {
                    dto.IsSucess = false;
                    return dto;
                }

                dto.Data = new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Dob = user.Dob,
                    RoleId = user.RoleId,
                    CreateAt = user.CreateAt,
                    Address = user.Address,
                    Status = user.Status,
                    Avatar = user.Avatar
                };

                dto.IsSucess = true;
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
            }
            return dto;
        }

    }
}
