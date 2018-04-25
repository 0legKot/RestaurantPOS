using System;

namespace RestaurantPOS.Models
{
    public class TableSeat
    {
        public TableSeat()
        {
            Id = new Guid(); 
        }

        public Guid Id { get;  }
        public Order Order { get; set; }
        
    }
}

   
