using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ShippingServices.Models
{
    public enum ShippingVendor
    {
        [EnumMember(Value = "JNE")]
        JNE,
        [EnumMember(Value = "J&T")]
        JNT,
        [EnumMember(Value = "SiCepat")]
        SiCepat
    }

}