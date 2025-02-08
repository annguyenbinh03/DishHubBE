using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Group6.NET1704.SW392.AIDiner.Common.DTO;

namespace Group6.NET1704.SW392.AIDiner.Services.Contract
{
    public interface IIngredientService
    {
        public Task<ResponseDTO> GetAllIngredients();
    }
}
