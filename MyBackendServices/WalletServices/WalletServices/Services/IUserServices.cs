using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletServices.Models;

namespace WalletServices.Services
{
    public interface IUserServices
    {
         Task<User> GetById(int id);
    }
}