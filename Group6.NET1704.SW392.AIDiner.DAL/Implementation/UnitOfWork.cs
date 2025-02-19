using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Data;

namespace Group6.NET1704.SW392.AIDiner.DAL.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private DishHub5Context _context;

    public UnitOfWork(DishHub5Context context)
    {
        _context = context;
    }
    
    
    public async Task<int> SaveChangeAsync()
    {
        return await _context.SaveChangesAsync();
    }
}