using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletServices.Models
{
    public class User
    {
        public int userId { get; set; }
        public string name { get; set; } = null!;
        public string email { get; set; } = null!;
    }
}