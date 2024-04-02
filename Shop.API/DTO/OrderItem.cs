namespace Shop.API.DTO
{
    public class OrderItem
    {
        public Item Replaced_with { get; set; } = new();
    }
    public class Item
    {
        public int Product_id { get; set; }
        public int Quantity { get; set; }
    }
}
