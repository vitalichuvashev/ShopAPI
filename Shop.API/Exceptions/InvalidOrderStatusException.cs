namespace Shop.API.Exceptions
{
    public class InvalidOrderStatusException: Exception
    {
        public InvalidOrderStatusException(Guid orderID) : base($"Invalid status for order with ID \"{orderID}\"") { }
    }
}
