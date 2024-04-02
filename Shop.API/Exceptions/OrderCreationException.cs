namespace Shop.API.Exceptions
{
    public class OrderCreationException:Exception
    {
        public OrderCreationException(Guid id):base($"Can't create new order with ID \"{id}\"") { }
    }
}
