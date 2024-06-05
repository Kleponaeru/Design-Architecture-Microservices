using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingServices.DTO
{
    public class WalletUpdateSaldoDTO
    {
        public string? Username { get; set; }
        public decimal Saldo { get; set; }
    }
}