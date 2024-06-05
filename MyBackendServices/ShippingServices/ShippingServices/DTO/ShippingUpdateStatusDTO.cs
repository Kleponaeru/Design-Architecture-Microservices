using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingServices.DTO
{
    public class ShippingUpdateStatusDTO
    {
        public int ShippingId { get; set; }
        public string ShippingStatus { get; set; } = null!;
    }
}