using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingServices.DAL.Interfaces
{
    public interface ICrud
    {
         public interface ICrud<T>
        {
            IEnumerable<T> GetAll();
            T GetById(string username);
            T Insert(T obj);
            void UpdateStatus(T obj);
            void Delete(int obj);


        }
    }
}