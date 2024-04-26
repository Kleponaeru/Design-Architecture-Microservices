using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogServices.DTO
{
    public class CategoryUpdateDto
    {
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
    }
}