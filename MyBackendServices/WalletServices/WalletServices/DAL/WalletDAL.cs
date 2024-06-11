using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using WalletServices.DAL.Interfaces;
using WalletServices.DTO;
using WalletServices.Models;

namespace WalletServices.DAL
{
    public class WalletDAL : IWallet
    {
        private string GetConnectionString()
        {

            return @"Data Source=.\SQLExpress;Initial Catalog=WalletDb;Integrated Security=true;";
            // return _configuration.GetConnectionString("DefaultConnection");
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"DELETE FROM Wallets 
                               WHERE WalletId = @WalletId";
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

        public IEnumerable<Wallet> GetAll()
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Wallets order by WalletId";
                return connection.Query<Wallet>(strSql); ;
            }
        }

        public string EncryptPassword(string password)
        {
            // Example: Use SHA256 for hashing
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        public Wallet Insert(Wallet obj)
        {
            // Encrypt the password
            string encryptedPassword = EncryptPassword(obj.Password);

            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"INSERT INTO Wallets (Username, Password, FullName, Saldo, WalletType, UserId) VALUES (@Username, @Password, @FullName, @Saldo, @WalletType, @UserId); SELECT @@IDENTITY;";
                var param = new
                {
                    Username = obj.Username,
                    Password = encryptedPassword, // Use the encrypted password
                    FullName = obj.FullName,
                    Saldo = obj.Saldo,
                    WalletType = obj.WalletType,
                    UserId = obj.UserId,
                };
                try
                {
                    var id = connection.ExecuteScalar<int>(strSql, param);
                    obj.WalletId = id;
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
        public void Update(Wallet obj)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE Wallet SET Saldo = @Saldo WHERE WalletId = @WalletId";
                var param = new { Saldo = obj.Saldo, WalletId = obj.WalletId };
                var result = connection.Execute(strSql, param);
                if (result == 0)
                {
                    throw new ArgumentException("Failed to update wallet");
                }
            }
        }

        public Wallet TopUp(Wallet obj)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"
            UPDATE Wallets 
            SET Saldo = Saldo + @TopUpAmount 
            WHERE Username = @Username;
            SELECT * FROM Wallets WHERE Username = @Username;";

                var param = new
                {
                    TopUpAmount = obj.Saldo,
                    Username = obj.Username,
                };

                try
                {
                    connection.Open();
                    var updatedWallet = connection.QuerySingleOrDefault<Wallet>(strSql, param);

                    if (updatedWallet == null)
                    {
                        throw new InvalidOperationException("Failed to top up the wallet. Wallet not found.");
                    }

                    return updatedWallet;
                }
                catch (SqlException sqlEx)
                {
                    throw new ArgumentException($"SQL Error: {sqlEx.Message} - Error Code: {sqlEx.Number}", sqlEx);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error: {ex.Message}", ex);
                }
            }
        }

        public Wallet GetByUsername(string username)
        {
            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"SELECT * FROM Wallets WHERE Username = @Username";
                var param = new { Username = username };

                // Execute the query asynchronously
                var wallets = connection.QueryFirstOrDefault<Wallet>(strSql, param);

                if (wallets == null)
                {
                    throw new ArgumentException("Data tidak ditemukan");
                }

                return wallets;
            }
        }



        public void UpdateSaldoAfterOrder(WalletUpdateSaldoDTO walletUpdateSaldoDTO)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                var strSql = @"UPDATE Wallets SET Saldo = @Saldo WHERE Username = @Username";
                var param = new { Saldo = walletUpdateSaldoDTO.Saldo, Username = walletUpdateSaldoDTO.Username };
                try
                {
                    connection.Execute(strSql, param);
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