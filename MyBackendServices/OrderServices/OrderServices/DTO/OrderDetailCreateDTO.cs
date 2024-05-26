using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.DTO
{
    public class OrderDetailCreateDTO
    {
        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? Username { get; set; }
        // public int WalletId { get; set; }
        // public decimal Saldo { get; set; }

    }
}