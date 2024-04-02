using Shop.Domain;

namespace Shop.API.Services.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAll();
    }
}
