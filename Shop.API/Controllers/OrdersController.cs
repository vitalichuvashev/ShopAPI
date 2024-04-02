using Microsoft.AspNetCore.Mvc;
using Shop.API.DTO;
using Shop.API.Services;
using Shop.API.Services.Interfaces;

namespace Shop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)=> this._orderService = orderService;

        ///api/orders - create a new order
        [HttpPost]
        public IActionResult CreateOrder()
        {
            try
            {
                var order = this._orderService.CreateNew();
                return Ok(order);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        ///api/orders/:order_id - get order details
        [HttpGet("{order_id}")]
        public IActionResult GetOrderDetails(string order_id)
        {
            if (Guid.TryParse(order_id, out var orderId))
            {
                try
                {
                    var order = this._orderService.GetOrder(orderId);
                    return Ok(order);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest("Wrong GUID format");
        }

        //api/orders/:order_id - update an order
        [HttpPatch("{order_id}")]
        public IActionResult PatchOrderStatus(string order_id, [FromBody] OrderStatus orderStatus)
        {
            if (Guid.TryParse(order_id, out var orderId))
            {
                if (ValidateOrderStatus(orderStatus))
                {
                    try
                    {
                        var order = this._orderService.ChangeStatus(orderId, orderStatus);
                        return Ok(order);
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }
                }
                else
                    return BadRequest("Wrong order status");
            }
            return BadRequest("Wrong GUID format");
        }

        //api/orders/:order_id/products
        [HttpGet("{order_id}/products")]
        public IActionResult GetOrderProducts(string order_id)
        {
            if (Guid.TryParse(order_id, out var orderId))
            {
                try
                {
                    var products = this._orderService.GetOrderProducts(orderId);
                    if (products.Count > 0)
                    {
                        return Ok(products);
                    }
                    else
                        return Ok($"Order ID: \"{orderId}\". Order product list is empty");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest("Wrong GUID format");
        }

        [HttpPost("{order_id}/products")]
        public IActionResult PostProducts(string order_id, [FromBody] List<int> idList)
        {
            if (Guid.TryParse(order_id, out var orderId))
            {
                if(idList.Count > 0)
                {
                    if(idList.Count == idList.Distinct().Count())// check if there no duplicated values
                    {
                        try
                        {
                            var order = this._orderService.AddProducts(orderId, idList);
                            return Ok(order);
                        }
                        catch(Exception e)
                        { return BadRequest(e.Message); }
                    }
                    else
                        return BadRequest($"There is duplicated values for order ID: {orderId}");
                }
                else
                    return BadRequest($"Empty product list for order ID: {orderId}");
            }
            return BadRequest("Wrong GUID format");
        }



        ///api/orders/:order_id/products/:product_id
        [HttpPatch("{order_id}/products/{product_id}")]
        public IActionResult PatchOrder(string order_id, string product_id, [FromBody] object value)
        {
            if (Guid.TryParse(order_id, out var orderId))
            {
                if (Guid.TryParse(product_id, out var productID))
                {
                    var message = string.Empty;
                    OrderQuantity? orderQuantity = null;
                    try
                    {
                        orderQuantity = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderQuantity>(value?.ToString() ?? string.Empty);
                    }
                    catch(Exception e)
                    { return BadRequest($"Wrong json format {e.Message}"); }

                    if(ValidateOrderQuantity(orderQuantity))
                    {
                        try
                        {
                            var order = this._orderService.UpdateProductQuantity(orderId, productID, orderQuantity.Quantity);
                            return Ok(order);
                        }
                        catch (Exception e)
                        { return BadRequest(e.Message); }
                    }

                    DTO.OrderItem? orderItem = null;
                    try
                    {
                        orderItem = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.OrderItem>(value?.ToString() ?? string.Empty);
                    }
                    catch (Exception e)
                    { return BadRequest($"Wrong json format {e.Message}"); }
                    
                    if (ValidateOrderItem(orderItem))
                    {
                        try
                        {
                            var order = this._orderService.ReplaceOrderItem(orderId, productID, orderItem);
                            return Ok(order);
                        }
                        catch (Exception e)
                        { return BadRequest(e.Message); }
                    }

                    return BadRequest("Wrong data format");

                }
                else
                    return BadRequest("Product ID wrong GUID format");
            }
            return BadRequest("Order ID wrong GUID format");
        }
        private bool ValidateOrderQuantity(DTO.OrderQuantity? orderQuantity)
        {
            if (orderQuantity != null)
            {
                if (orderQuantity.Quantity > 0)
                    return true;
            }
            return false;
        }
        private bool ValidateOrderItem(DTO.OrderItem? orderItem)
        {
            if (orderItem != null)
            {
                if (orderItem.Replaced_with != null)
                {
                    if (orderItem.Replaced_with.Product_id > 0)
                    {
                        if (orderItem.Replaced_with.Quantity > 0)
                            return true;
                    }
                }
            }
            return false;
        }
        private bool ValidateOrderStatus(OrderStatus orderStatus)
        {
            if(orderStatus != null)
            {
                if(!string.IsNullOrEmpty(orderStatus.status))
                {
                    if (orderStatus.status == OrderService.OrderStatus.PAID.ToString())
                        return true;
                   
                }
            }
            return false;
        }
    }
}
