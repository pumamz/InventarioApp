using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using ENTITIES;
namespace SERVICES
{
    public class APIService
    {
        private readonly HttpClient _httpClient;

        public APIService() {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:3000/api/");
        }

        public async Task<Producto> ObtenerProducto(int id) {
            HttpResponseMessage response = await _httpClient.GetAsync($"productos/{id}");
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            Producto producto = JsonConvert.DeserializeObject<Producto>(json);
            return producto;
        }

        public async Task<List<Producto>> getAllProduct() {
            HttpResponseMessage response = await _httpClient.GetAsync($"productos");
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Producto>>(json);
        }
    }
}
