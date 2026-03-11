namespace RestaurantApi.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public int customerId { get; set; }
        public DateTime OrderDate { get; set; }

        public List<OrderItem> Items { get; set; }
    }
}
