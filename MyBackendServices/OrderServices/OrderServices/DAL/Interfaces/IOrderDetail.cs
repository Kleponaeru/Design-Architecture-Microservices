using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderServices.Models;
using static OrderServices.DAL.Interfaces.ICrud;

namespace OrderServices.DAL.Interfaces
{
    public interface IOrderDetail : ICrud <OrderDetail>
    {
        
    }
}