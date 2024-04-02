using Microsoft.EntityFrameworkCore;
using Shop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastructure.Interfaces
{
    public interface IProductRepository
    {

        List<Product> GetAll();

        List<Product> GetProducts(params int[] ids);

        Product? GetProduct(int productID);
        
    }
}
