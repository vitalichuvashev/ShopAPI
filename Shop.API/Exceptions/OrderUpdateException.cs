namespace Shop.API.Exceptions
{
    public class OrderUpdateException: Exception
    {
        public OrderUpdateException(Guid orderID) : base($"Order with ID \"{orderID}\" didn't updated")
        { }
    }
}
