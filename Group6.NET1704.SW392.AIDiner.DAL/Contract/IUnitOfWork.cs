using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.DAL.Repositories.Interfaces;

namespace Group6.NET1704.SW392.AIDiner.DAL.Contract;

public interface IUnitOfWork
{
    IRestaurantRepository Restaurants {  get; }
    IUserRepository Users {  get; }

    IGenericRepository<Order> Orders { get; }

    IGenericRepository<Table> Tables { get; }
    IGenericRepository<OrderDetail> OrderDetails { get; }
    IGenericRepository<DishIngredient> DishIngredientRepository { get; }
    public IGenericRepository<Payment> Payments { get; }
    public IGenericRepository<Dish> Dishes { get; }
    public Task<int> SaveChangeAsync();

}