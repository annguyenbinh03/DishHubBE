using Group6.NET1704.SW392.AIDiner.Common.Request;
using Group6.NET1704.SW392.AIDiner.Common.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.DAL.Repositories.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<RestaurantsWithTablesResponse>> GetAllWithTablesAsync();
        Task<IEnumerable<RestaurantGetAllResponse>> GetAll();
        Task<RestaurantCreationReponse> Create(RestaurantCreationRequest request);
        Task<RestaurantUpdateReponse> Update(int id, RestaurantUpdateRequest request);
    }
}
