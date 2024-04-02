using Microsoft.AspNetCore.Mvc;
using Shop.API.Services;
using Shop.Domain;
using Shop.API.Services.Interfaces;

namespace Shop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;
        public ProductsController(IProductService productService) => this._productService = productService;

        [HttpGet]
        public List<Product> Get()
        {
            return this._productService.GetAll();
        }
    }
}
