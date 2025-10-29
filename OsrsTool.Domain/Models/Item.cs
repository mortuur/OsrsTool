using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsrsTool.Domain.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public long BuyPrice { get; set; }
        public long SellPrice { get; set; }
        public bool MembersOnly { get; set; }
        public DateTime LastUpdated { get; set; }
    }

}
