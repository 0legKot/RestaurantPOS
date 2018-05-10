using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantPOS.Models
{
    public class OrderInfo:ICloneable
    {
        private OrderInfo() { }
        public OrderInfo(Order order, Guid table, List<SeatNumber> tableSeats)
        {
            Order = order;
            TableId = table;
            TableSeatsNumbers = tableSeats;
        }
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Order Order { get; set; }
    
        public Guid TableId { get; set; }
        public List<SeatNumber> TableSeatsNumbers { get; set; }

        public object Clone()
        {
            return new OrderInfo { Id = Id, Order=(Order)Order.Clone(), TableId = TableId, TableSeatsNumbers = TableSeatsNumbers };
        }
    }
}
