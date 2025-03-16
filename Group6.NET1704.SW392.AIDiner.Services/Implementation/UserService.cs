using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.AdminDTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.Common.Model.UserModel;
using Group6.NET1704.SW392.AIDiner.Common.UserModel;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Group6.NET1704.SW392.AIDiner.Services.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public async Task<ResponseDTO> GetAllUser(int page, int size, string? search, string? sortBy, string? sortOrder)
        {
            ResponseDTO dto = new ResponseDTO();
            try
            {
                    var query = _userRepository.GetQueryable().AsQueryable();

                query = query.Include(u => u.Role);

                if (!string.IsNullOrEmpty(search))
                {
                    query = query.Where(u => u.Username.Contains(search) || u.Email.Contains(search));
                }

                query = sortBy?.ToLower() switch
                {
                    "username" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(u => u.Username) : query.OrderBy(u => u.Username),
                    "email" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                    "dob" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(u => u.Dob) : query.OrderBy(u => u.Dob),
                    _ => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(u => u.CreateAt) : query.OrderBy(u => u.CreateAt)
                };

                int totalUsers = await query.CountAsync();
                int totalPages = (int)Math.Ceiling(totalUsers / (double)size);
                List<User> users = await query.Skip((page - 1) * size).Take(size).ToListAsync();

                dto.Data = new
                {
                    TotalPages = totalPages,
                    CurrentPage = page,
                    PageSize = size,
                    Users = users.Select(u => new UserDTO
                    {
                        Id = u.Id,
                        Username = u.Username,
                        FullName = u.FullName,
                        Email = u.Email,
                        Dob = u.Dob,
                        PhoneNumber = u.PhoneNumber,
                        RoleId = u.RoleId,
                        RoleName = u.Role.Name,
                        CreateAt = u.CreateAt,
                        Address = u.Address,
                        IsDeleted = u.IsDeleted,
                        Avatar = u.Avatar
                    }).ToList()
                };
                dto.IsSucess = true;
                dto.BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                dto.IsSucess = false;
                dto.BusinessCode = BusinessCode.EXCEPTION;
                dto.Data = ex.Message;
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
                    IsDeleted = user.IsDeleted,
                    Avatar = user.Avatar
                };

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

        public async Task<ResponseDTO> UpdateUserForAdmin(int id, UpdateUserModel userDTO)
        {
            try
            {
                var existingUser = await _userRepository.GetById(id);
                if (existingUser == null)
                {
                    return new ResponseDTO
                    {
                        IsSucess = false,
                        BusinessCode = BusinessCode.NOT_FOUND,
                    };
                }

                if (userDTO.Username != null) existingUser.Username = userDTO.Username;
                if (userDTO.FullName != null) existingUser.FullName = userDTO.FullName;
                if (userDTO.Email != null) existingUser.Email = userDTO.Email;
                if (userDTO.Dob.HasValue) existingUser.Dob = userDTO.Dob.Value;
                if (userDTO.PhoneNumber != null) existingUser.PhoneNumber = userDTO.PhoneNumber;
                if (userDTO.Address != null) existingUser.Address = userDTO.Address;
                if (userDTO.IsDeleted.HasValue) existingUser.IsDeleted = userDTO.IsDeleted.Value;
                if (userDTO.Avatar != null) existingUser.Avatar = userDTO.Avatar;
                if (userDTO.Password != null) existingUser.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password); 

                if (await _userRepository.ExistsAsync(u => u.Username == existingUser.Username && u.Id != existingUser.Id))
                    return new ResponseDTO { IsSucess = false, BusinessCode = BusinessCode.USERNAME_ALREADY_EXISTS };

                if (await _userRepository.ExistsAsync(u => u.Email == existingUser.Email && u.Id != existingUser.Id))
                    return new ResponseDTO { IsSucess = false, BusinessCode = BusinessCode.EMAIL_ALREADY_EXISTS };

                if (await _userRepository.ExistsAsync(u => u.PhoneNumber == existingUser.PhoneNumber && u.Id != existingUser.Id))
                    return new ResponseDTO { IsSucess = false, BusinessCode = BusinessCode.PHONE_ALREADY_EXISTS };

                await _userRepository.Update(existingUser);
                await _unitOfWork.SaveChangeAsync();


                return new ResponseDTO
                {
                    IsSucess = true,
                    BusinessCode = BusinessCode.UPDATE_SUCESSFULLY,
                    Data = new UpdateUserModel
                    {
                        //Id = existingUser.Id,
                        Username = existingUser.Username,
                        FullName = existingUser.FullName,
                        Email = existingUser.Email,
                        Dob = existingUser.Dob,
                        PhoneNumber = existingUser.PhoneNumber,
                        Address = existingUser.Address,
                        IsDeleted = existingUser.IsDeleted,
                        Avatar = existingUser.Avatar,
                    }
                };
            }
            catch
            {
                return new ResponseDTO
                {
                    IsSucess = false,
                    BusinessCode = BusinessCode.EXCEPTION,
                };
            }
        }

        public async Task<ResponseDTO> UpdateProfileUser(int id, UpdateProfileUserModel userDTO)
        {
            try
            {
                var existingUser = await _userRepository.GetById(id);
                if (existingUser == null)
                {
                    return new ResponseDTO
                    {
                        IsSucess = false,
                        BusinessCode = BusinessCode.NOT_FOUND,
                    };
                }

                if (userDTO.FullName != null) existingUser.FullName = userDTO.FullName;
                if (userDTO.Email != null) existingUser.Email = userDTO.Email;
                if (userDTO.Password != null) existingUser.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
                if (userDTO.Dob.HasValue) existingUser.Dob = userDTO.Dob.Value;
                if (userDTO.PhoneNumber != null) existingUser.PhoneNumber = userDTO.PhoneNumber;
                if (userDTO.Address != null) existingUser.Address = userDTO.Address;
                if (userDTO.Avatar != null) existingUser.Avatar = userDTO.Avatar;

                if (await _userRepository.ExistsAsync(u => u.Username == existingUser.Username && u.Id != existingUser.Id))
                    return new ResponseDTO { IsSucess = false, BusinessCode = BusinessCode.USERNAME_ALREADY_EXISTS };

                if (await _userRepository.ExistsAsync(u => u.Email == existingUser.Email && u.Id != existingUser.Id))
                    return new ResponseDTO { IsSucess = false, BusinessCode = BusinessCode.EMAIL_ALREADY_EXISTS };

                if (await _userRepository.ExistsAsync(u => u.PhoneNumber == existingUser.PhoneNumber && u.Id != existingUser.Id))
                    return new ResponseDTO { IsSucess = false, BusinessCode = BusinessCode.PHONE_ALREADY_EXISTS };

                await _userRepository.Update(existingUser);
                await _unitOfWork.SaveChangeAsync();


                return new ResponseDTO
                {
                    IsSucess = true,
                    BusinessCode = BusinessCode.UPDATE_SUCESSFULLY,
                    Data = new UpdateProfileUserModel
                    {
                        FullName = existingUser.FullName,
                        Email = existingUser.Email,
                        Dob = existingUser.Dob,
                        PhoneNumber = existingUser.PhoneNumber,
                        Address = existingUser.Address,
                        Avatar = existingUser.Avatar,

                    }
                };
            }
            catch
            {
                return new ResponseDTO
                {
                    IsSucess = false,
                    BusinessCode = BusinessCode.EXCEPTION,
                };
            }
        }

        public async Task<ResponseIsSucessDTO> DeleteUserForAdmin(int id)
        {
            ResponseIsSucessDTO dto = new ResponseIsSucessDTO();
            try
            {
                var user = await _userRepository.GetById(id);
                if (user == null)
                {
                    dto.IsSucess = false;

                    return dto;
                }
                user.IsDeleted = true;
                await _userRepository.Update(user);
                await _unitOfWork.SaveChangeAsync();
                dto.IsSucess = true;

            }
            catch (Exception ex)
            {
                dto.IsSucess = false;

            }


            return dto;
        }

        public async Task<ResponseDTO> CreateUserAsync(AdminCreateAccountDTO model)
        {
            ResponseDTO response = new ResponseDTO();
            try
            {
                if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.INVALID_INPUT;
                    response.message = "Invalid input data";
                }
                if (await _userRepository.ExistsAsync(u => u.Username == model.Username))
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.USERNAME_ALREADY_EXISTS;
                    response.message = "Username already exists";
                    return response;
                }

                if (await _userRepository.ExistsAsync(u => u.Email == model.Email))
                {
                    response.IsSucess = false;
                    response.BusinessCode = BusinessCode.EMAIL_ALREADY_EXISTS;
                    response.message = "Email already exists";
                    return response;
                }

                User newUser = new User
                {
                    Username = model.Username,
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Dob = model.Dob,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Avatar = model.Avatar,
                    IsDeleted = false,
                    CreateAt = TimeZoneUtil.GetCurrentTime()
                };

                await _userRepository.Insert(newUser);
                await _unitOfWork.SaveChangeAsync();

                response.IsSucess = true;
                response.BusinessCode = BusinessCode.CREATE_SUCCESS;
                response.message = "User created successfully";
                response.Data = newUser;
            }
            catch (Exception ex)
            {
                response.IsSucess = false;
                response.BusinessCode = BusinessCode.EXCEPTION;
                response.message = "Error: " + ex.Message;
            }
            return response;
        }
    }
}
