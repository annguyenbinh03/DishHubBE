using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Data;
using Group6.NET1704.SW392.AIDiner.DAL.Repositories;
using Group6.NET1704.SW392.AIDiner.DAL.Repositories.Interfaces;

namespace Group6.NET1704.SW392.AIDiner.DAL.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private DishHub5Context _context;

    public IRestaurantRepository Restaurants { get; private set; }

    public UnitOfWork(DishHub5Context context)
    {
        _context = context;
        Restaurants = new RestaurantRepository(context);
    }
    
    
    public async Task<int> SaveChangeAsync()
    {
        return await _context.SaveChangesAsync();
    }
}