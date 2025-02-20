using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.DAL.Repositories.Interfaces;

namespace Group6.NET1704.SW392.AIDiner.DAL.Contract;

public interface IUnitOfWork
{
    IRestaurantRepository Restaurants {  get; }
    public Task<int> SaveChangeAsync();
    IGenericRepository<DishIngredient> DishIngredientRepository { get; }
}