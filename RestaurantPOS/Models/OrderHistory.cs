using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantPOS.Models
{
    public class OrderHistory
    {
        IList<OrderInfo> Orders { get;  } = new List<OrderInfo>();

        public OrderHistory(/*AppDbContext appDb*/)
        {
            //dbContext = appDb;
        }
        public void AddOrder(Order order, Guid tableId, List<SeatNumber> tableSeatsNumbers)
        {
            //Orders.Add(new OrderInfo(order, tableId, tableSeat.Number));
        }
        public void AddSeatToOrder(Guid orderId, int tableSeatsNumber)
        {
            var order=Orders.FirstOrDefault(x=>x.Order.Id==orderId);
            if (order == null || order.Order.ClosedDate != new DateTime()) return;
            order.TableSeatsNumbers.Add(new SeatNumber { Id = Guid.NewGuid(), Number=tableSeatsNumber });
        }
        public OrderInfo GetOrderInfo(Guid OrderInfoId)
        {
            var tmpOrderInfo = Orders.FirstOrDefault(o => o.Id == OrderInfoId);
            return tmpOrderInfo?.Clone() as OrderInfo;

        }
        public IList<OrderInfo> GetOrders()
        {
            List<OrderInfo> result = new List<OrderInfo>();
            //foreach (OrderInfo oi in dbContext.OrderHistory)
            //    result.Add(oi);
            foreach (Guid oi in Orders.Select(x => x.Id))
                result.Add(GetOrderInfo(oi));
            return result;
        }
    }
}
