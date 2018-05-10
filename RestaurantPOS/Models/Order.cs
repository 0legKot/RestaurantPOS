using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantPOS.Models
{

    public partial class Order:ICloneable
    {
        private Order() { }
        public Order(string waiter )
        {
            Id = Guid.NewGuid();
            Waiter = waiter;
            State = OrderState.Active;
            OpenedDate = DateTime.Now;
        }

        public string Waiter { get; set; }

        public Guid Id { get; private set; }
        public IList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public decimal Tips { get; set; }
        public decimal Discount { get; set; }
        public OrderState State { get;  set; }
        IList<OrderItem> CloneOrderItems() {
            List<OrderItem> clone = new List<OrderItem>();
            foreach (OrderItem oi in OrderItems)
                clone.Add((OrderItem)oi.Clone());
            return clone;
        }
        public void Void()
        {
            State = OrderState.Voided;
            Close();
        }
        public void Ready() => State = OrderState.Ready;

        public void Pay()
        {
            State = OrderState.Paid;
            Close();
        }

        public decimal TotalPrice => OrderItems.Select(oi => oi.Price).Sum()+Tips-Discount;
        public DateTime OpenedDate { get; private set; }
        public DateTime ClosedDate { get;  set; }
         void Close() => ClosedDate = DateTime.Now;

        public object Clone()
        {
            return new Order { Id = Id,
            OrderItems = CloneOrderItems(),
            Tips = Tips,
            Discount = Discount,
            State = State,
            OpenedDate = OpenedDate,
            ClosedDate = ClosedDate,
            Waiter=Waiter
        };
        }
    }
}

   
