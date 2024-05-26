using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderServices.DTO;
using OrderServices.Models;

namespace OrderServices.Services
{
    public interface IWalletServices
    {
        Task<IEnumerable<Wallet>> GetAllUser();
        Task<Wallet> GetSaldo(string username);
        Task UpdateWalletBySaldo(WalletUpdateSaldoDTO walletUpdateSaldoDTO);
    }
}