namespace Shop.API.Exceptions
{
    public class OrderNotFoundException: Exception
    {
        public OrderNotFoundException(Guid orderID):base($"Order with ID \"{orderID}\" didn't found")
        { }
            

    }
}
