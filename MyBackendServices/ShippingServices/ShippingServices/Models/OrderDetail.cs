using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingServices.Models
{
    public class OrderDetail
    {
        public int orderDetailId { get; set; }
        public int orderHeaderId { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
        public string? customerName { get; set; }
        public DateTime orderDate { get; set; }
        public string? username { get; set; }
    }
}