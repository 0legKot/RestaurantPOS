using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantPOS.Models
{
    public class OrderHistory
    {
        IList<OrderInfo> Orders { get; set; } = new List<OrderInfo>();
        public void AddOrder(Order order, Guid tableId, List<int> tableSeatsNumbers)
        {
            Orders.Add(new OrderInfo(order, tableId, tableSeatsNumbers));
        }
        public OrderInfo GetOrderInfo(Guid OrderInfoId)
        {
            var tmpOrderInfo = Orders.Where(o => o.Id == OrderInfoId).FirstOrDefault();
            return (OrderInfo)tmpOrderInfo.Clone();

        }
        public IList<OrderInfo> GetOrders()
        {
            List<OrderInfo> result = new List<OrderInfo>();
            foreach (Guid oi in Orders.Select(x => x.Id))
                result.Add(GetOrderInfo(oi));
            return result;
        }
    }
}
