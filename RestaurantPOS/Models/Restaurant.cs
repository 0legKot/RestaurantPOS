using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantPOS.Models
{
    public class Restaurant
    {
        public int Rows { get; } = 10;
        public int Columns { get; } = 10;
        public Table[,] TablesGrid { get; set; } 
        public Restaurant()
        {
            TablesGrid = new Table[Rows, Columns];
            TablesGrid[2,3]=new Table(2,3);
            TablesGrid[5, 6] = new Table(5,6);
        }
        public Restaurant(int _rows, int _columns)
        {
            Rows = _rows;
            Columns = _columns;
            TablesGrid = new Table[Rows, Columns];
        }
    }
}
