using System;

namespace RestaurantPOS.Models
{
    public partial class OrderItem
    {
        Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public decimal Price { get; set; }
        public byte[] CustomerMedia { get; set; }
    }
}

   
