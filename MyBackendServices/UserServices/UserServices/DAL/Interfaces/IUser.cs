using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserServices.Models;
using static UserServices.DAL.Interfaces.ICrud;

namespace UserServices.DAL.Interfaces
{
    public interface IUser : ICrud <User>
    {

    }
}