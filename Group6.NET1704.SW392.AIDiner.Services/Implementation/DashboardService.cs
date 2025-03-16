using Group6.NET1704.SW392.AIDiner.Common.DTO;
using Group6.NET1704.SW392.AIDiner.Common.DTO.BusinessCode;
using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.Services.Contract;
using Group6.NET1704.SW392.AIDiner.Services.Util;
using Microsoft.EntityFrameworkCore;
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
            var firstDayOfMonth = today.AddDays(1 - today.Day);
            var firstDayOfYear = today.AddDays(1 - today.DayOfYear); 
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
                    Data = new
                    {
                        TopDailyDishes = new List<object>(),
                        TopMonthlyDishes = new List<object>(),
                        TopYearlyDishes = new List<object>(),
                        TopAllTimeDishes = new List<object>()
                    },
                    BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY,
                    message = "Không có bàn nào trong nhà hàng này!"
                };
            }

            var orders = await _unitOfWork.Orders.GetAllDataByExpression(
                filter: o => tableIds.Contains(o.TableId) && o.Status == "completed",
                pageNumber: null,
                pageSize: null
            );

            var orderIds = orders.Items.Select(o => o.Id).ToList();
            var orderDetails = await _unitOfWork.OrderDetails.GetQueryable()
                .Where(od => orderIds.Contains(od.OrderId))
                .Include(od => od.Dish)
                .ThenInclude(d => d.Category)
                .ToListAsync();

            var now = TimeZoneUtil.GetCurrentTime();

            List<object> GetTopDishes(Func<Order, bool> filterCondition)
            {
                return orderDetails
                    .Where(od => orders.Items.Any(o => o.Id == od.OrderId && filterCondition(o)))
                    .GroupBy(od => new
                    {
                        od.DishId,
                        DishName = od.Dish.Name,
                        CategoryName = od.Dish.Category.Name,
                        od.Dish.Price,
                        od.Dish.Image
                    })
                    .Select(g => new
                    {
                        g.Key.DishId,
                        Name = g.Key.DishName,
                        CategoryName = g.Key.CategoryName,
                        Price = g.Key.Price,
                        Image = g.Key.Image,
                        Sold = g.Sum(od => od.Quantity)
                    })
                    .OrderByDescending(d => d.Sold)
                    .Take(5)
                    .ToList<object>();
            }


            var topDailyDishes = GetTopDishes(o => o.CreatedAt.Value.Date == now.Date);
            var topMonthlyDishes = GetTopDishes(o => o.CreatedAt.Value.Year == now.Year && o.CreatedAt.Value.Month == now.Month);
            var topYearlyDishes = GetTopDishes(o => o.CreatedAt.Value.Year == now.Year);
            var topAllTimeDishes = GetTopDishes(o => true);

            return new ResponseDTO
            {
                IsSucess = true,
                Data = new
                {
                    TopDailyDishes = topDailyDishes,
                    TopMonthlyDishes = topMonthlyDishes,
                    TopYearlyDishes = topYearlyDishes,
                    TopAllTimeDishes = topAllTimeDishes
                },
                BusinessCode = BusinessCode.GET_DATA_SUCCESSFULLY,
                message = "Lấy dữ liệu thành công!"
            };
        }
    }

}
