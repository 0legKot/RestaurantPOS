using System;

namespace RestaurantPOS.Models
{
    public class TableSeat:ICloneable
    {
        public TableSeat()
        {
            Id = Guid.NewGuid();
            
        }

        public Guid Id { get; private set; }
        public Order Order { get; set; }

        public object Clone()
        {
            return new TableSeat
            {
                Id = this.Id,
                Order = this.Order
            };


        }
    }
}

   
