using Shop.API.Exceptions;
using Shop.Domain;
using Shop.Infrastructure.Interfaces;
using Shop.API.Services.Interfaces;

namespace Shop.API.Services
{
    public class ProductService: IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository) => this._productRepository = productRepository;
        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns>return product list</returns>
        public List<Product> GetAll()=>_productRepository.GetAll();
        
    }
}
