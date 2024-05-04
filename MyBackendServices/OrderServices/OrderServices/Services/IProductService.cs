using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderServices.Models;

namespace OrderServices.Services
{
    public interface IProductServices
    {
        Task <IEnumerable<Product>> GetAllProducts();
        Task <Product> GetProductById(int id);
    }
}