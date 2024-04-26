using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServices.DAL.Interfaces
{
    public interface ICrud
    {
        public interface ICrud<T>
        {
            IEnumerable<T> GetAll();
            T GetById(int id);

            void Insert(T obj);
            void Update(T obj);
            void Delete(int obj);


        }
    }
}