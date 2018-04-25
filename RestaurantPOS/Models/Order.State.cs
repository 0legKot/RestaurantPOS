namespace RestaurantPOS.Models
{

    public partial class Order
    {
        public enum OrderState
        {
            Active=0,
            Voided=1,
            Paid=2
        }
    }
}
