using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletServices.DAL.Interfaces;
using WalletServices.DTO;
using WalletServices.Models;
using static WalletServices.DAL.Interfaces.ICrud;

namespace WalletServices.DAL
{
    public interface IWallet : ICrud <Wallet>
    {
        void UpdateSaldoAfterOrder (WalletUpdateSaldoDTO walletUpdateSaldoDTO);
    }
}