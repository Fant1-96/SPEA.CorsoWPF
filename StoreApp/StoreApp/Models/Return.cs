using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Models
{
    public class Return
    {
        public int Id { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Reason { get; set; }
        public decimal RefundAmount { get; set; }
        public ICollection<OrderProduct> ReturnedProduct { get; set; } = new List<OrderProduct>();
    }
}
