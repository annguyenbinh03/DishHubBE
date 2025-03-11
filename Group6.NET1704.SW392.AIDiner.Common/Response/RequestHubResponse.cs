using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group6.NET1704.SW392.AIDiner.Common.Response
{
    public class RequestHubResponse
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string TableName { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string? Note { get; set; }

        public string Status { get; set; } = null!;
    }
}
