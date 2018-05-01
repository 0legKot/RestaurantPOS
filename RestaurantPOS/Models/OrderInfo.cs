using System;
using System.Collections.Generic;

namespace RestaurantPOS.Models
{
    public class OrderInfo
    {
        public OrderInfo(Order order, Table table, List<TableSeat> tableSeats)
        {
            Order = order;
            Table = table;
            TableSeats = tableSeats;
        }
        public Guid Id { get; } = Guid.NewGuid();
        public Order Order { get; set; }
        public Table Table { get; set; }
        public List<TableSeat> TableSeats { get; set; }

    }
}
