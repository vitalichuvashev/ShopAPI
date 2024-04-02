using Shop.Domain;
using Shop.Infrastructure.Interfaces;

namespace Shop.Infrastructure
{
    public class ProductRepository: IProductRepository
    {
        private readonly DatabaseContext _dbContext;
        public ProductRepository(DatabaseContext dbContext) => _dbContext = dbContext;

        public List<Product> GetAll()
        {
            return _dbContext.Products.ToList();
        }

        public List<Product> GetProducts(params int[] ids)
        {
            
            return ids.Length > 0 ? _dbContext.Products.Where(p => ids.Contains(p.ID)).ToList() : new();
        }
        public Product? GetProduct(int productID)
        {

            return _dbContext.Products.FirstOrDefault(p=>p.ID==productID);
        }
    }
}
