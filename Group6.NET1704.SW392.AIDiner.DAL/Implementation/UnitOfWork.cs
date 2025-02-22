using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Data;
using Group6.NET1704.SW392.AIDiner.DAL.Repositories.Interfaces;
using Group6.NET1704.SW392.AIDiner.DAL.Models;

namespace Group6.NET1704.SW392.AIDiner.DAL.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private DishHub5Context _context;

    public IRestaurantRepository Restaurants { get; private set; }

    public UnitOfWork(DishHub5Context context, IRestaurantRepository restaurantRepository)
    {
        _context = context;
        DishIngredientRepository = new GenericRepository<DishIngredient>(_context);
        Restaurants = restaurantRepository;
    }

    public IGenericRepository<DishIngredient> DishIngredientRepository { get; private set; }

    IGenericRepository<DishIngredient> IUnitOfWork.DishIngredientRepository => throw new NotImplementedException();

    public async Task<int> SaveChangeAsync()
    {
        return await _context.SaveChangesAsync();
    }
}