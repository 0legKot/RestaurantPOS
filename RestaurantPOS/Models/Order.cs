using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantPOS.Models
{

    public partial class Order
    {
        public Order( decimal tips = 0)
        {
            Id = Guid.NewGuid();
            Tips = tips;
            State = OrderState.Active;
            OpenedDate = DateTime.Now;
        }
        public Guid Id { get; }
        public IList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public decimal Tips { get; set; }
        public decimal Discount { get; set; } = 0;
        public OrderState State { get;  set; }
        public void Void()
        {
            State = OrderState.Voided;
            Close();
        }

        public void Pay()
        {
            State = OrderState.Paid;
            Close();
        }

        public decimal TotalPrice => OrderItems.Select(oi => oi.Price).Sum()+Tips-Discount;
        public DateTime OpenedDate { get; }
        public DateTime ClosedDate { get; private set; }
         void Close() => ClosedDate = DateTime.Now;
    }
}

   
