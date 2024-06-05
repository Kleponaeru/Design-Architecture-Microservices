using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingServices.DTO
{
    public class ShippingDTO
    {
        public int ShippingId { get; set; }
        public int OrderDetailId { get; set; }
        public string ShippingVendor { get; set; } = null!;
        public DateTime ShippingDate { get; set; }
        public string ShippingStatus { get; set; } = null!;
        public int BeratBarang { get; set; }
        public decimal BiayaShipping { get; set; }
    }
}