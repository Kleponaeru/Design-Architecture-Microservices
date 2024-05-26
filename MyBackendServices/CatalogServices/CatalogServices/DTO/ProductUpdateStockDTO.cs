using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogServices.DTO
{
    public class ProductUpdateStockDTO
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}