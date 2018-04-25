using System;

namespace RestaurantPOS.Models
{
    public class Table
    {
        const byte seatsInTable = 4;
        public Table()
        {
            Id = new Guid();
            TableSeats = new TableSeat[seatsInTable] 
            {
                new TableSeat(),
                new TableSeat(),
                new TableSeat(),
                new TableSeat()
            };
        }

        public Guid Id { get; }
        public bool IsActive { get; set; } = true;
        public TableSeat[] TableSeats { get; }
    }

}


