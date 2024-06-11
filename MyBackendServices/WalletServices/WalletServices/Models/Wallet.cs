using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletServices.Models
{
    public class Wallet
    {
        public int WalletId { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public decimal Saldo { get; set; }
         public string WalletType { get; set; } = null!;
        public int UserId { get; set; }
         

    }
}