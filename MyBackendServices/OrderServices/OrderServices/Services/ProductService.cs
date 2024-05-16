using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CatalogServices.DTO;
using OrderServices.Models;

namespace OrderServices.Services
{
    public class ProductServices : IProductServices
    {
        private readonly HttpClient _httpClient;
        public ProductServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7002");
        }
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var response = await _httpClient.GetAsync("/api/products");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(result);
                if (products == null)
                {
                    throw new ArgumentException("Cannot get products");
                }

                return products;
            }
            else
            {
                throw new ArgumentException($"Cannot get products - httpstatus: {response.StatusCode}");
            }
        }

        public async Task<Product> GetProductById(int id)
        {
            var response = await _httpClient.GetAsync($"/api/products/{id}");
            if (response.IsSuccessStatusCode)
            {
                var results = await response.Content.ReadAsStringAsync();
                var product = JsonSerializer.Deserialize<Product>(results);
                if (product == null)
                {
                    throw new ArgumentException("Cannot get products");
                }
                return product;
            }

            else
            {
                throw new ArgumentException($"Cannot get products - httpstatus: {response.StatusCode}");
            }
        }

        public async Task UpdateProductByStock(ProductUpdateStockDTO productUpdateStockDTO)
        {
            ///api/products/updatestocks
            var json = JsonSerializer.Serialize(productUpdateStockDTO);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("api/products/updatestocks", data);
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException($"Cannot get products - httpstatus: {response.StatusCode}");
            }
        }
    }
}