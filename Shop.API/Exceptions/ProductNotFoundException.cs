namespace Shop.API.Exceptions
{
    public class ProductNotFoundException: Exception
    {
        public ProductNotFoundException(int productID): base($"Product with ID: \"{productID}\" didn't found") { }
    }
}
