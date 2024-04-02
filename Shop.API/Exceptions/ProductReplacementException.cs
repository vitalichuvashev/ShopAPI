namespace Shop.API.Exceptions
{
    public class ProductReplacementException: Exception
    {
        public ProductReplacementException():base("Can't replace same product") { }
    }
}
