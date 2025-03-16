using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Group6.NET1704.SW392.AIDiner.Services.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseDTO> GetSalesInfo(int restaurantId)
        {
            var tables = await _unitOfWork.Tables.GetAllDataByExpression(
                filter: t => t.RestaurantId == restaurantId, 
                pageNumber: null,
                pageSize: null
            );

            var tableIds = tables.Items.Select(t => t.Id).ToList();
            if (!tableIds.Any())
            {
                return new ResponseDTO
                {
                    IsSucess = true,
                    Data = new { DailySales = 0, MonthlySales = 0, YearlySales = 0 },
                    BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY,
                    message = "Không có bàn nào trong nhà hàng này!"
                };
            }

            var orders = await _unitOfWork.Orders.GetAllDataByExpression(
                filter: o => tableIds.Contains(o.TableId) && o.Status == "completed", 
                pageNumber: null,
                pageSize: null
            );
            var currentTime = TimeZoneUtil.GetCurrentTime(); 
            var today = currentTime.Date; 
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var firstDayOfYear = new DateTime(today.Year, 1, 1);

            var salesData = new
            {
                DailySales = orders.Items
                    .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value.Date == today)
                    .Sum(o => o.TotalAmount),

                MonthlySales = orders.Items
                    .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value >= firstDayOfMonth)
                    .Sum(o => o.TotalAmount),

                YearlySales = orders.Items
                    .Where(o => o.CreatedAt.HasValue && o.CreatedAt.Value >= firstDayOfYear)
                    .Sum(o => o.TotalAmount)
            };

            return new ResponseDTO
            {
                IsSucess = true,
                Data = salesData,
                BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY, 
                message = "Thống kê doanh thu thành công!"
            };
        }

        public async Task<ResponseDTO> GetTopDishesInfo(int restaurantId)
        {
            ResponseDTO response = new ResponseDTO();
            response.IsSucess = true;
            response.Data = new List<object>
            {
                new { id = 2, name = "Bánh mì thịt", categoryName = "Bánh mì", price = 25000, image = "https://res.cloudinary.com/dkxnaypwm/image/upload/v1740110198/banh_mi_thit_g4bcwn.jpg", soldCount = 200 },
                new { id = 4, name = "Cơm tấm", categoryName = "Cơm", price = 55000, image = "https://res.cloudinary.com/dkxnaypwm/image/upload/v1740110200/com_tam_rapvqc.jpg", soldCount = 180 },
                new { id = 1, name = "Phở bò", categoryName = "Món nước", price = 60000, image = "https://res.cloudinary.com/dkxnaypwm/image/upload/v1740110199/pho_bo_sed2mt.jpg", soldCount = 150 },
                new { id = 7, name = "Hủ tiếu", categoryName = "Món nước", price = 50000, image = "https://res.cloudinary.com/dkxnaypwm/image/upload/v1740110202/hu_tieu_wfj6k3.jpg", soldCount = 140 },
                new { id = 10, name = "Chè bắp", categoryName = "Tráng miệng", price = 20000, image = "https://res.cloudinary.com/dkxnaypwm/image/upload/v1740110203/che_bap_ujn9sj.jpg", soldCount = 130 },
                new { id = 3, name = "Bún chả", categoryName = "Món nước", price = 50000, image = "https://res.cloudinary.com/drp7eohjg/image/upload/v1740589001/mhfsnh5cdgjseyzcl219.jpg", soldCount = 120 },
                new { id = 6, name = "Gỏi cuốn", categoryName = "Món khai vị", price = 30000, image = "https://res.cloudinary.com/dkxnaypwm/image/upload/v1740110201/goi_cuon_g6yr3j.jpg", soldCount = 110 },
                new { id = 5, name = "Bánh xèo", categoryName = "Món chiên", price = 40000, image = "https://res.cloudinary.com/dkxnaypwm/image/upload/v1740110200/banh_xeo_pwuhfr.jpg", soldCount = 90 },
                new { id = 8, name = "Bò lúc lắc", categoryName = "Món chính", price = 120000, image = "https://res.cloudinary.com/dkxnaypwm/image/upload/v1740110202/bo_luc_lac_pn7gmd.jpg", soldCount = 75 },
                new { id = 9, name = "Lẩu thái", categoryName = "Lẩu", price = 250000, image = "https://res.cloudinary.com/dkxnaypwm/image/upload/v1740110203/lau_thai_mfn2ls.jpg", soldCount = 60 }
            };
            return response;
        }
    }
}
