using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Models;

namespace FS.HotelBooking.DAL.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private DishHubContext _context;

    public UnitOfWork(DishHubContext context)
    {
        _context = context;
    }
    
    
    public async Task<int> SaveChangeAsync()
    {
        return await _context.SaveChangesAsync();
    }
}