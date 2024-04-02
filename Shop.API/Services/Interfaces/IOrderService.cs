using Shop.API.Exceptions;
using Shop.Domain;

namespace Shop.API.Services.Interfaces
{
    public interface IOrderService
    {
        Order ChangeStatus(Guid id, DTO.OrderStatus orderStatus);

        Order CreateNew();

        Order GetOrder(Guid id);

        List<OrderProduct> GetOrderProducts(Guid id);

        Order AddProducts(Guid orderID, List<int> productsIDs);

        Order UpdateProductQuantity(Guid orderID, Guid productID, int quantity);

        Order ReplaceOrderItem(Guid orderID, Guid productID, DTO.OrderItem orderItem);

    }
}
