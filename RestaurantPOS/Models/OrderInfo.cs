using System;
using System.Collections.Generic;

namespace RestaurantPOS.Models
{
    public class OrderInfo:ICloneable
    {
        private OrderInfo() { }
        public OrderInfo(Order order, Guid table, List<int> tableSeats)
        {
            Order = order;
            TableId = table;
            TableSeatsNumbers = tableSeats;
        }
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Order Order { get; set; }
        public Guid TableId { get; set; }
        public List<int> TableSeatsNumbers { get; set; }

        public object Clone()
        {
            return new OrderInfo { Id = Id, Order=(Order)Order.Clone(), TableId = TableId, TableSeatsNumbers = TableSeatsNumbers };
        }
    }
}
