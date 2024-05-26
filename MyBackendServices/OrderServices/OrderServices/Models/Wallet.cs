using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.Models
{
    public class Wallet
    {
        public int walletId { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public string? fullName { get; set; }
        public decimal saldo { get; set; }

    }
}