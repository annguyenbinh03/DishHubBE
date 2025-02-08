namespace Group6.NET1704.SW392.AIDiner.Common.DTO;

public class PagedResult<T> where T : class
{
    public List<T>? Items { get; set; }
    public int TotalPages { get; set; }
}