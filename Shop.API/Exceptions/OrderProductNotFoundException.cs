namespace Shop.API.Exceptions
{
    public class OrderProductNotFoundException: Exception
    {
        public OrderProductNotFoundException(Guid orderID, Guid productID) : base($"Product with ID: \"{productID}\" of order with ID: \"{orderID}\" didn't found") { }

    }
}
