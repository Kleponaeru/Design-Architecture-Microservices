using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShippingServices.Models;

namespace ShippingServices.Services
{
    public interface IOrderServices
    {
        Task <OrderDetail> GetOrderById(int id);
    }
}