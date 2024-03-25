// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using CatalogServices.Models;
// using System.Data.SqlClient;

// namespace CatalogServices.DAL.Interfaces
// {
//     public class ProductDAL : IProduct
//     {
//         private string GetConnectionString()
//         {

//             return @"Data Source=.\SQLExpress;Initial Catalog=CatalogDb;Integrated Security=true;";
//         }
//         public void Delete(int obj)
//         {
//             throw new NotImplementedException();
//         }

//         public IEnumerable<Product> GetAll()
//         {
//             List<Product> product = new List<Product>();
//             using (SqlConnection conn = new SqlConnection(GetConnectionString()))
//             {
//                 var strSql = @"SELECT * FROM Products order by ProductID";
//                 SqlCommand cmd = new SqlCommand(strSql, conn);
//                 conn.Open();
//                 SqlDataReader dr = cmd.ExecuteReader();

//                 if (dr.HasRows)
//                 {
//                     while (dr.Read())
//                     {
//                         Product products = new Product();
//                         products.ProductID = Convert.ToInt32(dr["ProductID"]);
//                         products.Name = dr["Name"].ToString();
//                         product.Add(products);
//                     }
//                 }
//                 dr.Close();
//                 cmd.Dispose();
//                 conn.Close();

//                 return product;
//             }
//         }

//             public IEnumerable<Product> GetByCategory(string name)
//             {
//                 throw new NotImplementedException();
//             }

//             public Product GetById(int id)
//             {
//                 throw new NotImplementedException();
//             }

//             public void Insert(Product obj)
//             {
//                 throw new NotImplementedException();
//             }

//             public void Update(Product obj)
//             {
//                 throw new NotImplementedException();
//             }


//         }
//     }
