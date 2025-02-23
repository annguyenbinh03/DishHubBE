using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Data;
using Group6.NET1704.SW392.AIDiner.DAL.Implementation;
using Group6.NET1704.SW392.AIDiner.DAL.Models;
using Group6.NET1704.SW392.AIDiner.DAL.Repositories.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly DishHub5Context _context;

    public IRestaurantRepository Restaurants { get; private set; }
    public IGenericRepository<DishIngredient> DishIngredientRepository { get; private set; }

    public UnitOfWork(DishHub5Context context, IRestaurantRepository restaurantRepository)
    {
        _context = context;
        DishIngredientRepository = new GenericRepository<DishIngredient>(_context);
        Restaurants = restaurantRepository;
    }

    public async Task<int> SaveChangeAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
