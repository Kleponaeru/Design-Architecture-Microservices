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
    public class OrderHeaderDAL : IOrderHeader
    {
        private string GetConnectionString()
        {

            return @"Data Source=.\SQLExpress;Initial Catalog=OrderDb;Integrated Security=true;";
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"DELETE FROM OrderHeaders 
                               WHERE OrderHeaderId = @OrderHeaderId";
                var param = new { OrderHeaderId = id };
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

        public IEnumerable<OrderHeader> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"
                SELECT o.*, c.CustomerName 
                FROM OrderHeaders o
                INNER JOIN Customers c ON o.CustomerId = c.CustomerId 
                ORDER BY o.OrderHeaderId";
                var orderHeaders = conn.Query<OrderHeader>(strSql);
                return orderHeaders;
            }
        }

        public OrderHeader GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"
                SELECT o.*, c.CustomerName 
                FROM OrderHeaders AS o
                INNER JOIN Customers AS c ON o.CustomerId = c.CustomerId 
                WHERE OrderHeaderId = @OrderHeaderId
                ORDER BY o.CustomerId";
                var param = new { OrderHeaderId = id };
                var orderHeader = conn.QueryFirstOrDefault<OrderHeader>(strSql, param);
                
                if (orderHeader == null)
                {
                    throw new ArgumentException("Data tidak ditemukan");
                }
                return orderHeader;
            }
        }

        public void Insert(OrderHeader obj)
        {
             using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO OrderHeaders (CustomerId, OrderDate) VALUES (@CustomerId, @OrderDate)";
                var param = new
                {
                    CustomerId = obj.CustomerId,
                    OrderDate = obj.OrderDate,
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

        public void Update(OrderHeader obj)
        {
             using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE OrderHeaders SET CustomerId = @CustomerId, 
                OrderDate = @OrderDate WHERE OrderHeaderId = @OrderHeaderId";
                var param = new
                {
                    OrderHeaderId = obj.OrderHeaderId,
                    CustomerId = obj.CustomerId,
                    OrderDate = obj.OrderDate,
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