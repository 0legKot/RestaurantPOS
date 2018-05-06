using System;

namespace RestaurantPOS.Models
{
    public partial class OrderItem:ICloneable
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; }
        public decimal Price { get; set; }
        public byte[] CustomerMedia { get; set; }

        public object Clone()
        {
            return new OrderItem { Id=Id,Name=Name,Price=Price,CustomerMedia=(byte[])CustomerMedia.Clone()};
        }
    }
}

   
