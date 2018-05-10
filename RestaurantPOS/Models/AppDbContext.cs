using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantPOS.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
               : base(options) { }
        public DbSet<OrderInfo> OrderHistory { get; set; }
        //public DbSet<SeatNumber> SeatNumbers { get; set; }
    }
}
