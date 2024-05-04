using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using OrderServices.DAL.Interfaces;
using OrderServices.Models;

namespace OrderServices.DAL
{
    public class CustomerDAL : ICustomer
    {
        // private readonly IConfiguration _configuration;
        // public CustomerDAL(IConfiguration configuration)
        // {
        //     _configuration = configuration;
        // }
        private string GetConnectionString()
        {

            return @"Data Source=.\SQLExpress;Initial Catalog=OrderDb;Integrated Security=true;";
            // return _configuration.GetConnectionString("DefaultConnection");
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"DELETE FROM Customers 
                               WHERE CustomerId = @CustomerId";
                var param = new { CustomerId = id };
                try
                {
                    conn.Execute(strSql, param);
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
            }
        }

        public IEnumerable<Customer> GetAll()
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Customers order by CustomerId";
                return connection.Query<Customer>(strSql); ;
            }
        }

        public Customer GetById(int id)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Customers WHERE CustomerId = @CustomerId";
                return connection.QueryFirstOrDefault<Customer>(strSql, new { CustomerId = id });
            }
        }

        public Customer Insert(Customer obj)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO Customers (CustomerName) VALUES (@CustomerName); SELECT @@IDENTITY;";
                // var param = new
                // {
                //     CustomerName = obj.CustomerName,
                // };
                try
                {
                    var newId = connection.ExecuteScalar<int>(strSql, new { CustomerName = obj.CustomerName });
                    obj.CustomerId = newId;
                    return obj;
                    // connection.Execute(strSql, param);
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
            }
        }

        public void Update(Customer obj)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE Customers SET CustomerName = @CustomerName WHERE CustomerId = @CustomerId";
                var param = new
                {
                    CustomerId = obj.CustomerId,
                    CustomerName = obj.CustomerName,
                };
                try
                {
                    conn.Execute(strSql, param);
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"Error: {sqlEx.Message} - {sqlEx.Number}");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}");
                }
            }

        }
    }
}