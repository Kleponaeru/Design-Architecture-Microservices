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
    public class OrderDetailDAL : IOrderDetail
    {
       private readonly IConfiguration _configuration;
        public OrderDetailDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private string GetConnectionString()
        {

            // return @"Data Source=.\SQLExpress;Initial Catalog=OrderDb;Integrated Security=true;";
            return _configuration?.GetConnectionString("DefaultConnection") ?? "DefaultConnectionString";
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"DELETE FROM OrderDetails 
                               WHERE OrderDetailId = @OrderDetailId";
                var param = new { OrderDetailId = id };
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

        public IEnumerable<OrderDetail> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"
                SELECT o.OrderDate, c.CustomerName, od.*
                FROM OrderHeaders o
                INNER JOIN Customers c ON o.CustomerId = c.CustomerId
                INNER JOIN OrderDetails od ON o.OrderHeaderId = od.OrderHeaderId
                ORDER BY od.OrderDetailId";
                var orderDetail = conn.Query<OrderDetail>(strSql);
                return orderDetail;
            }
        }

        public OrderDetail GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"
                SELECT o.OrderDate, c.CustomerName, od.*
                FROM OrderHeaders o
                INNER JOIN Customers c ON o.CustomerId = c.CustomerId
                INNER JOIN OrderDetails od ON o.OrderHeaderId = od.OrderHeaderId
                WHERE OrderDetailId = @OrderDetailId
                ORDER BY o.OrderHeaderId";
                var param = new { OrderDetailId = id };
                var orderDetail = conn.QueryFirstOrDefault<OrderDetail>(strSql, param);

                if (orderDetail == null)
                {
                    throw new ArgumentException("Data tidak ditemukan");
                }
                return orderDetail;
            }

        }

        public OrderDetail Insert(OrderDetail obj)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO OrderDetails (OrderHeaderId, ProductId, Quantity, Price) VALUES (@OrderHeaderId, @ProductId, @Quantity, @Price); SELECT @@IDENTITY;";
                var param = new
                {
                    OrderDetailId = obj.OrderDetailId,
                    OrderHeaderId = obj.OrderHeaderId,
                    ProductId = obj.ProductId,
                    Price = obj.Price,
                    Quantity = obj.Quantity,
                };
                try
                {
                    var id = connection.ExecuteScalar<int>(strSql, param);
                    obj.OrderDetailId = id;
                    return obj;
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

        public void Update(OrderDetail obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE OrderDetails SET OrderHeaderId = @OrderHeaderId, 
                ProductId = @ProductId, Price = @Price, Quantity = @Quantity WHERE OrderDetailId = @OrderDetailId";
                var param = new
                {
                    OrderDetailId = obj.OrderDetailId,
                    OrderHeaderId = obj.OrderHeaderId,
                    ProductId = obj.ProductId,
                    Price = obj.Price,
                    Quantity = obj.Quantity,
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
