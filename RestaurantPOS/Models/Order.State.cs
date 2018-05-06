namespace RestaurantPOS.Models
{

    public partial class Order
    {
        public enum OrderState
        {
            Active=3,
            Voided=1,
            Paid=2,
            Closed=0,
            Ready=4,
            InActive=5,
            Mixed=6
        }
    }
}
