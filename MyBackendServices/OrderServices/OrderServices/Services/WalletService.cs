using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using OrderServices.DTO;
using OrderServices.Models;

namespace OrderServices.Services
{
    public class WalletService : IWalletServices
    {
        private readonly HttpClient _httpClient;
        public WalletService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7003");
        }

        public async Task<IEnumerable<Wallet>> GetAllUser()
        {
            var response = await _httpClient.GetAsync("/api/user");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var wallet = JsonSerializer.Deserialize<IEnumerable<Wallet>>(result);
                if (wallet == null)
                {
                    throw new ArgumentException("Cannot get user");
                }

                return wallet;
            }
            else
            {
                throw new ArgumentException($"Cannot get products - httpstatus: {response.StatusCode}");
            }
        }


        public async Task<Wallet> GetSaldo(string username)
        {
            var response = await _httpClient.GetAsync($"/api/wallet/{username}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var wallet = JsonSerializer.Deserialize<Wallet>(result);
                if (wallet == null)
                {
                    throw new ArgumentException("Cannot get wallet");
                }
                return wallet;
            }
            else
            {
                throw new ArgumentException($"Cannot get wallet - httpstatus: {response.StatusCode}");
            }
        }

        public async Task UpdateWalletBySaldo(WalletUpdateSaldoDTO walletUpdateSaldoDTO)
        {
            var jsonContent = JsonSerializer.Serialize(walletUpdateSaldoDTO);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("/api/wallet/updateAfterOrder", content); // Using PUT instead of POST
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException($"Cannot update wallet - httpstatus: {response.StatusCode}");
            }
        }


    }
}