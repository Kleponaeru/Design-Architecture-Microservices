using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WalletServices.Models;

namespace WalletServices.Services
{
    public class UserServices : IUserServices
    {
        private readonly HttpClient _httpClient;
        public UserServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7005");
        }

        public async Task<User> GetById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/find/user/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<User>(result);
                if (user == null)
                {
                    throw new ArgumentException("Cannot get wallet");
                }
                return user;
            }
            else
            {
                throw new ArgumentException($"Cannot get wallet - httpstatus: {response.StatusCode}");
            }
        }

    }
}