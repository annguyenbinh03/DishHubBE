using Group6.NET1704.SW392.AIDiner.DAL.Contract;
using Group6.NET1704.SW392.AIDiner.DAL.Data;

namespace FS.HotelBooking.DAL.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private AIDinerDbContext _context;

    public UnitOfWork(AIDinerDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<int> SaveChangeAsync()
    {
        return await _context.SaveChangesAsync();
    }
}