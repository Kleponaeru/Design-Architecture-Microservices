using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.DTO
{
    public class OrderHeaderCreateDTO
    {
        public int CustomerId { get; set; }
        // public string? Username { get; set; }
        public DateTime OrderDate { get; set; }
        // public List<OrderDetailCreateDTO> OrderDetails { get; set; } = new List<OrderDetailCreateDTO>();

    }
}