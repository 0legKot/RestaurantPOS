using System;

namespace RestaurantPOS.Models
{
    public class Table:ICloneable
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
            IsActive = true;
        }
        public Order.OrderState GeneralState {
            get {
                if (!IsActive) return Order.OrderState.InActive;
                var prevState = Order.OrderState.Closed;

                foreach (TableSeat ts in TableSeats)
                    if (ts.Order!=null&&prevState!=Order.OrderState.Closed&& ts.Order.State != prevState) return Order.OrderState.Mixed;
                    else prevState = ts.Order?.State??prevState;
                return prevState;
            }
            
        } 
        public int Row { get; set; }
        public int Column { get; set; }
        public Guid Id { get; private set; }
        public bool IsActive {
            get;set;
        }
        public TableSeat[] TableSeats { get; private set; }
        TableSeat[] CloneTableSeats()
        {
            TableSeat[] tableSeats = new TableSeat[seatsInTable];
            for (int i = 0; i < TableSeats.Length; i++)
                tableSeats[i] = (TableSeat)TableSeats[i].Clone();
            return tableSeats;
        }
        public object Clone()
        {
            return new Table(Row, Column) {Id=Id,IsActive=IsActive,TableSeats=CloneTableSeats()};
        }
        //public (int,int) Position { get; set; }
    }

}


