using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? CustomerName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string Username { get; set; } = null!;

    }
}