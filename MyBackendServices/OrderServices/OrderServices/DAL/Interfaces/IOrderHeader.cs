using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderServices.DAL.Interfaces;
using OrderServices.Models;
using static OrderServices.DAL.Interfaces.ICrud;

namespace OrderServices.DAL
{
    public interface IOrderHeader : ICrud <OrderHeader>
    {
        
    }
}