using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogServices.Models;

namespace CatalogServices.DAL.Interfaces
{
    public interface IProduct:ICrud<Product>
    {
        IEnumerable<Product> GetByCategory (string name);
        IEnumerable<Product> GetByCategoryId (int id);
    }
}