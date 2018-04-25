using System;

namespace RestaurantPOS.Models
{
    public partial class OrderItem
    {
        Guid Id { get; set; }
        string Name { get; set; }
        public decimal Price { get; set; }
        public Media CustomerMedia { get; set; }
    }
}

   
