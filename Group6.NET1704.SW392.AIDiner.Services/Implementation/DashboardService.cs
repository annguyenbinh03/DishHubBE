using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        public async Task<ResponseDTO> GetSalesInfo(int restaurantId)
        {
           ResponseDTO response = new ResponseDTO();
            response.IsSucess = true;
            response.Data = new
            {
                dailySales = 2300000,
                monthlySales = 95300000,
                yearlySales = 342000000
            };
            return response;
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
