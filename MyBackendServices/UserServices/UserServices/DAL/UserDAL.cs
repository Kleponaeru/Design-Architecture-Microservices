using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using UserServices.DAL.Interfaces;
using UserServices.Models;

namespace UserServices.DAL
{
    public class UserDAL : IUser
    {
        private string GetConnectionString()
        {

            return @"Data Source=.\SQLExpress;Initial Catalog=UserDb;Integrated Security=true;";
            // return _configuration.GetConnectionString("DefaultConnection");
        }
        public void Delete(int obj)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"DELETE FROM Users
                               WHERE UserId = @UserId";
                var param = new { UserId = obj };
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

        public IEnumerable<User> GetAll()
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Users ORDER BY UserId";
                return connection.Query<User>(strSql); ;
            }
        }

        public User GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Users WHERE UserId = @UserId";
                var param = new { UserId = id };

                // Execute the query asynchronously
                var users = connection.QueryFirstOrDefault<User>(strSql, param);

                if (users == null)
                {
                    throw new ArgumentException("User tidak ditemukan");
                }

                return users;
            }
        }

        public User Insert(User obj)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO Users (Name, Email) VALUES (@Name, @Email); SELECT @@IDENTITY;";
                var param = new
                {
                    UserId = obj.UserId,
                    Name = obj.Name, 
                    Email = obj.Email,
                };
                try
                {
                    var id = connection.ExecuteScalar<int>(strSql, param);
                    obj.UserId = id;
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

        public void Update(User obj)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE Users SET Name = @Name,  Email =  @Email WHERE UserId = @UserId";
                var param = new { UserId = obj.UserId, Name = obj.Name, Email = obj.Email };
                var result = connection.Execute(strSql, param);
                if (result == 0)
                {
                    throw new ArgumentException("Failed to update User");
                }
            }
        }
    }
}