namespace Shop.Domain
{
    

    public class Order
    {

        public Amount Amount { get; set; } = new();
        public Guid ID { get; set; }= new();
        public List<OrderProduct> Products { get; set; }=new();
        public string Status { get; set; } = string.Empty;
        
    }
   
    public class Amount
    {
        public string Discount { get; set; } = "0.00";
        public string Paid { get; set; } = "0.00";
        public string Returns { get; set; } = "0.00";
        public string Total { get; set; } = "0.00";

        
    }
}


