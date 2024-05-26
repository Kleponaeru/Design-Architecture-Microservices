using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletServices.DAL.Interfaces
{
    public interface ICrud
    {
        public interface ICrud<T>
        {
            IEnumerable<T> GetAll();
            T GetByUsername(string username);
            T Insert(T obj);
            T TopUp(T obj);
            void Update(T obj);
            void Delete(int obj);


        }
    }
}