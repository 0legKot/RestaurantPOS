using System;

namespace RestaurantPOS.Models
{
    public class Table
    {
        const byte seatsInTable = 4;
        public Table(int row,int column)
        {
            Id = Guid.NewGuid();// new Guid();
            Row = row;
            Column = column;
            TableSeats = new TableSeat[seatsInTable] 
            {
                new TableSeat(),
                new TableSeat(),
                new TableSeat(),
                new TableSeat()
            };
        }
        public int Row { get; set; }
        public int Column { get; set; }
        public Guid Id { get; }
        public bool IsActive { get; set; } = true;
        public TableSeat[] TableSeats { get; }
        //public (int,int) Position { get; set; }
    }

}


