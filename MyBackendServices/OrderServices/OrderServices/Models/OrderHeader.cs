using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Models
{
    public class OrderHeader
    {
        public int OrderHeaderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? CustomerName { get; set; } = null!;
        // public string Username { get; set; } = null!;
    }
}