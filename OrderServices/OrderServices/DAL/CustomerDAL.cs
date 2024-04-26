using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderServices.DAL.Interfaces;
using OrderServices.Models;

namespace OrderServices.DAL
{
    public class CustomerDAL : ICustomer
    {
        private string GetConnectionString()
        {

            return @"Data Source=.\SQLExpress;Initial Catalog=OrderDb;Integrated Security=true;";
        }
        public void Delete(int obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetAll()
        {
            throw new NotImplementedException();
        }

        public Customer GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Customer obj)
        {
            throw new NotImplementedException();
        }

        public void Update(Customer obj)
        {
            throw new NotImplementedException();
        }
    }
}