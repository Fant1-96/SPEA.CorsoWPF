using System;
using System.Collections.Generic;
namespace StoreApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
        //public ICollection<Return> Returns { get; set; } = new List<Return>();
    }
}