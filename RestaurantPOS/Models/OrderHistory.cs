using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantPOS.Models
{
    public class OrderHistory
    {
        IList<OrderInfo> Orders { get; set; } = new List<OrderInfo>();
        public void AddOrder(Order order, Table table, List<TableSeat> tableSeats)
        {
            Orders.Add(new OrderInfo(order,table,tableSeats));
        }

        public IList<OrderInfo> GetOrders() => Orders.ToList();
    }
}
