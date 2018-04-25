using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantPOS.Models
{
    public class OrderHistory
    {
        IList<Order> Orders { get; set; } = new List<Order>();
        public void AddOrder(Order order) => Orders.Add(order);
        public IList<Order> GetOrders() => Orders.ToList();
    }
}
