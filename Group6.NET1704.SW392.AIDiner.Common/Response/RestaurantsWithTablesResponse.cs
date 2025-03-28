using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Common.Response
{
    public class RestaurantsWithTablesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; }      
        public List<RestaurantTables> Tables { get; set; } = new List<RestaurantTables>();
    }

    public class RestaurantTables
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Status { get; set; }
    }
}
