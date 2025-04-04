﻿using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.Request;
using Group6.NET1704.SW392.AIDiner.Common.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.Contract
{
    public interface IRestaurantService
    {
        Task<ResponseDTO> GetAllWithTablesAsync();
        Task<ResponseDTO> GetAll();
        Task<ResponseDTO> Create(RestaurantCreationRequest request);
        Task<ResponseDTO> Update(int id, RestaurantUpdateRequest request);
        Task<ResponseDTO> Delete(int id);
    }
}
