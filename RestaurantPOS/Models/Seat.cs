using System;

namespace RestaurantPOS.Models
{
    public class TableSeat
    {
        public TableSeat()
        {
            Id = Guid.NewGuid(); 
        }

        public Guid Id { get;  }
        public Order Order { get; set; }
        
    }
}

   
