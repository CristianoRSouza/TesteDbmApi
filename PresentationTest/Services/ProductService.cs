using Newtonsoft.Json;
using PresentationTest.Dtos;
using System.Text;

namespace PresentationTest.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly ILogger<ProductService> _logger;

        public ProductService(HttpClient httpClient, IConfiguration configuration, ILogger<ProductService> logger)
        {
            _httpClient = httpClient;
            _apiBaseUrl = configuration["ApiBaseUrl"];
            _logger = logger;
        }

        public async Task<List<ProductRequest>> GetProductsAsync()
        {
            var products = new List<ProductRequest>();

            HttpResponseMessage response = await _httpClient.GetAsync($"{_apiBaseUrl}product");

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<List<ProductRequest>>(jsonResponse);
            }

            return products;
        }

        public async Task<ProductRequest?> GetProductByIdAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_apiBaseUrl}product/{id}");

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProductRequest>(jsonResponse);
            }

            return null;
        }
        public async Task<bool> CreateProductAsync(ProductRequest product)
        {
            var jsonContent = JsonConvert.SerializeObject(product);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{_apiBaseUrl}product", content);
            string responseText = await response.Content.ReadAsStringAsync();

            _logger.LogInformation($"Resposta da API: {responseText}");
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateProductAsync(int id, ProductRequest product)
        {
            var jsonContent = JsonConvert.SerializeObject(product);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


            HttpResponseMessage response = await _httpClient.PutAsync($"{_apiBaseUrl}product", content);
            string responseText = await response.Content.ReadAsStringAsync();

            _logger.LogInformation($"Resposta da API: {responseText}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_apiBaseUrl}product/{id}");
            return response.IsSuccessStatusCode;
        }
    }

}
