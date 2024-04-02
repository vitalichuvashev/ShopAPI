using Shop.API.Exceptions;
using Shop.API.Services.Interfaces;
using Shop.Domain;
using Shop.Infrastructure.Interfaces;

namespace Shop.API.Services
{
    public class OrderService : IOrderService
    {
        public enum OrderStatus { NEW, PAID }

        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        { 
            this._orderRepository = orderRepository; 
            this._productRepository = productRepository;
        }

        /// <summary>
        /// Changes order status 
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="orderStatus">Data transfer object with status value</param>
        /// <exception cref="InvalidOrderStatusException">Wrong order status exception</exception>
        /// <exception cref="OrderUpdateException">Order didn't updated exception</exception>
        /// <exception cref="OrderNotFoundException">Order didn't found exception</exception>
        /// <exception cref="DatabaseException">Internal entity framework exception</exception>
        public Order ChangeStatus(Guid id, DTO.OrderStatus orderStatus)
        {
            var order = this._orderRepository.LoadByID(id);
            if (order != null)
            {
                if (orderStatus.status == OrderStatus.PAID.ToString())
                {
                    order.Status = orderStatus.status;
                    order.Amount.Paid = order.Amount.Total;
                    UpdateOrder(order);
                    return order;
                }
                else
                    throw new InvalidOrderStatusException(id);
            }
            else
                throw new OrderNotFoundException(id);
        }
        /// <summary>
        /// Creates empty order, otherwise throw exception 
        /// </summary>
        /// <returns>returns new order with status: NEW</returns>
        /// <exception cref="OrderCreationException"></exception>
        /// <exception cref="DatabaseException">Internal entity framework exception</exception>
        public Order CreateNew()
        {
            Order newOrder = new();
            newOrder.Status = OrderStatus.NEW.ToString();

            int savedCount = this._orderRepository.AddNew(newOrder);
            if (savedCount != 0)
            {
                return newOrder;
            }
            else
                throw new OrderCreationException(newOrder.ID);    
        }
        /// <summary>
        /// Gets order by ID, otherwise throw exception
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>returns saved Order</returns>
        /// <exception cref="OrderNotFoundException">Order didn't found exception</exception>
        public Order GetOrder(Guid id)
        {
            var order = this._orderRepository.LoadByID(id) ?? throw new OrderNotFoundException(id);
            var list = order.Products.Select(p => p.Price);
            return order;
        }
        /// <summary>
        /// Gets order product list by order ID
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>returns order products</returns>
        /// <exception cref="OrderNotFoundException">Order didn't found exception</exception>
        public List<OrderProduct> GetOrderProducts(Guid id)
        {
            var order = this._orderRepository.LoadByID(id) ?? throw new OrderNotFoundException(id);
            return order.Products;
        }

        /// <summary>
        /// Adds product ID list to order
        /// </summary>
        /// <param name="orderID">Order ID</param>
        /// <param name="productsIDs">Product ID list</param>
        /// <returns>returns order with product list</returns>
        /// <exception cref="InvalidOrderStatusException">Wrong order status exception</exception>
        /// <exception cref="OrderNotFoundException">Order didn't found exception</exception>
        /// <exception cref="DatabaseException">Internal entity framework exception</exception>
        public Order AddProducts(Guid orderID, List<int> productsIDs)
        {
            var order = this._orderRepository.LoadByID(orderID) ?? throw new OrderNotFoundException(orderID);
            if(order.Status == OrderStatus.NEW.ToString())
            {            
                var products = this._productRepository.GetProducts(productsIDs.ToArray());
                foreach (var product in products)
                {
                    var orderProduct = order.Products.FirstOrDefault(p => p.Product_id == product.ID);
                    if (orderProduct != null)// if same product already exist, increase quantity
                    {
                        orderProduct.Quantity += 1;
                    }
                    else// if doesn't exist, add new one
                    {
                        order.Products.Add(new OrderProduct()
                        {
                            Name = product.Name,
                            Price = product.Price,
                            Product_id = product.ID
                        });
                    }
                    
                }
                if (products.Count > 0)
                {
                    CalculateTotal(order);
                    UpdateOrder(order);
                }
            }
            else
                throw new InvalidOrderStatusException(orderID);

            return order;
        }
        /// <summary>
        /// Updates product quantity of related order
        /// </summary>
        /// <param name="orderID">Order ID</param>
        /// <param name="productID">Product ID</param>
        /// <param name="quantity">Product quantity</param>
        /// <returns>returns order with updated product quantity</returns>
        /// <exception cref="OrderNotFoundException">Order didn't found exception</exception>
        /// <exception cref="OrderProductNotFoundException">Product didn't found exception</exception>
        /// <exception cref="DatabaseException">Internal entity framework exception</exception>
        public Order UpdateProductQuantity(Guid orderID, Guid productID, int quantity)
        {
            var order = this._orderRepository.LoadByID(orderID) ?? throw new OrderNotFoundException(orderID);
            if (order.Status == OrderStatus.NEW.ToString())
            {
                var orderProduct= order.Products.FirstOrDefault(p => p.ID == productID) ?? throw new OrderProductNotFoundException(orderID, productID);
                if(orderProduct.Replaced_with != null)// if product replaced with another one, update quantity for replaced product 
                {
                    orderProduct.Replaced_with.Quantity = quantity;
                }
                else// otherwise, update quantity for current product
                    orderProduct.Quantity = quantity;

                CalculateTotal(order);
                UpdateOrder(order);
            }
            else
                throw new InvalidOrderStatusException(orderID);

            return order;
        }
        /// <summary>
        /// Replaces order product with new one
        /// </summary>
        /// <param name="orderID">Order ID</param>
        /// <param name="productID">Product ID</param>
        /// <param name="orderItem">Data transfer object with product ID and quantity</param>
        /// <returns></returns>
        /// <exception cref="OrderNotFoundException">Order didn't found exception</exception>
        /// <exception cref="OrderProductNotFoundException">Order product didn't found exception</exception>
        /// <exception cref="ProductNotFoundException">Product didn't found exception</exception>
        /// <exception cref="ProductReplacementException">Product duplicate replacement exception</exception>
        /// <exception cref="DatabaseException">Internal entity framework exception</exception>
        public Order ReplaceOrderItem(Guid orderID, Guid productID, DTO.OrderItem orderItem)
        {
            var order = this._orderRepository.LoadByID(orderID) ?? throw new OrderNotFoundException(orderID);
            if (order.Status == OrderStatus.NEW.ToString())
            {
                var orderProduct = order.Products.FirstOrDefault(p => p.ID == productID) ?? throw new OrderProductNotFoundException(orderID, productID);
                var product = this._productRepository.GetProduct(orderItem.Replaced_with.Product_id) ?? throw new ProductNotFoundException(orderItem.Replaced_with.Product_id);
                if (orderProduct.Product_id != orderItem.Replaced_with.Product_id)// check if product for replace doesn't equal to order product, otherwise exception 
                {
                    orderProduct.Replaced_with = new ProductItem() { Product_id = product.ID, Quantity = orderItem.Replaced_with.Quantity };
                }
                else
                    throw new ProductReplacementException();
                    
                CalculateTotal(order);
                UpdateOrder(order);
            }
            else
                throw new InvalidOrderStatusException(orderID);

            return order;
        }
        /// <summary>
        /// Updates order or throws exception 
        /// </summary>
        /// <param name="order">Order object</param>
        /// <exception cref="OrderUpdateException">Order didn't updated exception</exception>
        private void UpdateOrder(Order order)
        {
            int updatedCount = this._orderRepository.Update(order);
            if (updatedCount == 0)
            {
                throw new OrderUpdateException(order.ID);
            }    
        }
        private void CalculateTotal(Order order)
        {
            decimal total = 0;
            foreach(var orderProduct in order.Products)
            {
                if(orderProduct.Replaced_with == null)
                {
                    decimal price = Convert.ToDecimal(orderProduct.Price);
                    total += price * orderProduct.Quantity;
                }
                else// if product replaced, then calculate total with replaced product price
                {
                    var product = this._productRepository.GetProduct(orderProduct.Replaced_with.Product_id);
                    decimal price = Convert.ToDecimal(product.Price);
                    total += price * orderProduct.Replaced_with.Quantity;
                }
            }
            order.Amount.Total = total.ToString();
        }
    }
}
