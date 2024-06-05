using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Dapper;
using ShippingServices.DAL.Interfaces;
using ShippingServices.Models;

namespace ShippingServices.DAL
{
    public class ShippingDAL : IShipping
    {
        private string GetConnectionString()
        {

            return @"Data Source=.\SQLExpress;Initial Catalog=ShippingDb;Integrated Security=true;";
            // return _configuration.GetConnectionString("DefaultConnection");
        }
        public void Delete(int obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Shipping> GetAll()
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Shippings order by ShippingId";
                return connection.Query<Shipping>(strSql); ;
            }
        }

        public Shipping GetById(string username)
        {
            throw new NotImplementedException();
        }

        public Shipping Insert(Shipping obj)
        {
            using (var conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO Shippings (OrderDetailId, ShippingVendor, ShippingDate, ShippingStatus,  BeratBarang, BiayaShipping) VALUES  (@OrderDetailId, @ShippingVendor, @ShippingDate, @ShippingStatus, @BeratBarang, @BiayaShipping); SELECT @@IDENTITY;";
                var param = new
                {
                    OrderDetailId = obj.OrderDetailId,
                    ShippingVendor = obj.ShippingVendor,
                    ShippingDate = obj.ShippingDate,
                    ShippingStatus = obj.ShippingStatus,
                    BeratBarang = obj.BeratBarang,
                    BiayaShipping = obj.BiayaShipping,
                };
                try
                {
                    var id = conn.ExecuteScalar<int>(strSql, param);
                    obj.ShippingId = id;
                    return obj;
                    // conn.Execute(strSql, param);
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

        public void UpdateStatus(Shipping obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE Shippings SET ShippingStatus = @ShippingStatus WHERE ShippingId = @ShippingId";
                var param = new
                {
                    ShippingId = obj.ShippingId,
                    ShippingStatus = obj.ShippingStatus,
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

        public DateOnly GetEstimatedDate(int id)
        {
            return DateOnly.FromDateTime(DateTime.Now).AddDays(5);
        }
    
    }
}