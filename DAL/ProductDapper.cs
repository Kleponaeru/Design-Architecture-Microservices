using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CatalogServices.DAL.Interfaces;
using CatalogServices.Models;
using Dapper;

namespace CatalogServices.DAL.Interfaces
{
    public class ProductDapper : IProduct
    {
        private string GetConnectionString()
        {
            return @"Data Source=.\SQLEXPRESS;Initial Catalog=CatalogDb;Integrated Security=True";
            //return @"Server=localhost,1433;Initial Catalog=CatalogDb;User ID=sa;Password=Indonesia@2023;TrustServerCertificate=True;";
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"DELETE FROM Products 
                               WHERE ProductID = @ProductID";
                var param = new { ProductID = id };
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

        public IEnumerable<Product> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"
            SELECT p.*, c.categoryName 
            FROM Products p
            INNER JOIN Categories c ON p.CategoryID = c.CategoryID 
            ORDER BY p.ProductID";
                var product = conn.Query<Product>(strSql);
                return product;
            }
        }

        public Product GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT p.*, c.categoryName FROM products p
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID
                       WHERE ProductID = @ProductID";
                var param = new { ProductID = id };
                var product = conn.QueryFirstOrDefault<Product>(strSql, param);
                if (product == null)
                {
                    throw new ArgumentException("Data tidak ditemukan");
                }
                return product;
            }
        }
        public IEnumerable<Product> GetByCategoryId(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"
            SELECT p.*, c.categoryName 
            FROM Products p
            INNER JOIN Categories c ON p.CategoryID = c.CategoryID
            WHERE c.CategoryID = @CategoryId";
                var param = new { CategoryId = id };
                var products = conn.Query<Product>(strSql, param);
                return products;
            }
        }



        public IEnumerable<Product> GetByCategory(string name)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT p.*, c.categoryName 
               FROM Products p
               INNER JOIN Categories c ON p.CategoryID = c.CategoryID
               WHERE c.CategoryName LIKE '%' + @CategoryName + '%'";
                var param = new { CategoryName = name };
                var products = conn.Query<Product>(strSql, param);
                return products;
            }
        }

        public void Insert(Product obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO Products (Name, Description, Price, Quantity, CategoryID) VALUES (@Name, @Description, @Price, @Quantity, @CategoryID)";
                var param = new
                {
                    Name = obj.Name,
                    Description = obj.Description,
                    Price = obj.Price,
                    Quantity = obj.Quantity,
                    CategoryID = obj.CategoryID
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

        public void Update(Product obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE Products SET Name = @Name, Description = @Description, Price = @Price, Quantity = @Quantity, CategoryID = @CategoryID 
                       WHERE ProductID = @ProductID";
                var param = new
                {
                    Name = obj.Name,
                    Description = obj.Description,
                    Price = obj.Price,
                    Quantity = obj.Quantity,
                    CategoryID = obj.CategoryID,
                    ProductID = obj.ProductID,
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