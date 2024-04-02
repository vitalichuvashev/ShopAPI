namespace Shop.Domain
{
    public class OrderProduct : ProductItem
    {
        public Guid ID { get; set; } = new();
        public string Name { get; set; } = string.Empty;

        public string Price { get; set; } = string.Empty;

        public ProductItem? Replaced_with { get; set; } = default;

    }
    public class ProductItem
    {
        public int Product_id { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
