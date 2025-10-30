using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsrsTool.Domain.Entities
{
    public class ItemPriceHistory
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
        public int HighPrice { get; set; }
        public int LowPrice { get; set; }
    }
}
