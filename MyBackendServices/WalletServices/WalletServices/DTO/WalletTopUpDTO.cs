using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletServices.DTO
{
    public class WalletTopUpDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public decimal Saldo { get; set; }
    }
}