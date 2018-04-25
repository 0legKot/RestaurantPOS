using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantPOS.Models
{

    public partial class Order
    {
        public Order( decimal tips = 0, IEnumerable<OrderItem> orderItems=null)
        {
            Id = new Guid();
            foreach (OrderItem item in orderItems)
            OrderItems.Add(item);
            Tips = tips;
            State = OrderState.Active;
            OpenedDate = DateTime.Now;
        }

        public Guid Id { get; }
        public IList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public decimal Tips { get; set; }
        public OrderState State { get; private set; }
        public void Void() => State = OrderState.Voided;
        public void Pay() => State = OrderState.Paid;
        decimal TotalPrice => OrderItems.Select(oi => oi.Price).Sum();
        public DateTime OpenedDate { get; }
        public DateTime ClosedDate { get; private set; }
        public void Close() => ClosedDate = DateTime.Now;
    }
}

   
