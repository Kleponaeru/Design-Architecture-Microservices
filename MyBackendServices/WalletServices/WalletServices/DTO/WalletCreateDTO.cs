using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletServices.DTO
{
    public class WalletCreateDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public decimal Saldo { get; set; }
    }
}