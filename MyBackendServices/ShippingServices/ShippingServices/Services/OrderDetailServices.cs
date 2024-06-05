using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ShippingServices.Models;

namespace ShippingServices.Services
{
    public class OrderDetailServices : IOrderServices
    {
        private readonly HttpClient _httpClient;
        public OrderDetailServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7001");
        }

        public async Task<OrderDetail> GetOrderById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/orderDetails/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var orderDetails = JsonSerializer.Deserialize<OrderDetail>(result);
                if (orderDetails == null)
                {
                    throw new ArgumentException("Cannot get Order Details");
                }

                return orderDetails;
            }
            else
            {
                throw new ArgumentException($"Cannot get Order Details - httpstatus: {response.StatusCode}");
            }
        }
    }
}