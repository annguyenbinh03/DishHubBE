namespace Group6.NET1704.SW392.AIDiner.DAL.Contract;

public interface IUnitOfWork
{
    public Task<int> SaveChangeAsync();
}