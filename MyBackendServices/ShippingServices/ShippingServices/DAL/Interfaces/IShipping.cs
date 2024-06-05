using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShippingServices.Models;
using static ShippingServices.DAL.Interfaces.ICrud;

namespace ShippingServices.DAL.Interfaces
{
    public interface IShipping : ICrud<Shipping>
    {
        DateOnly GetEstimatedDate(int id);
    }
}