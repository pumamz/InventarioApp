using ENTITIES;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES
{
    public class ApiService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string _jwtToken = string.Empty;
        private readonly string _baseUrl = "http://localhost:3000/";

        public ApiService()
        {
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(_baseUrl);
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        private void ActualizarAuthHeader()
        {
            if (!string.IsNullOrEmpty(_jwtToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            }
        }

        private StringContent CrearContenidoJson(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var request = new LoginRequest { Email = email, Password = password };
            var response = await _httpClient.PostAsync("auth/login", CrearContenidoJson(request));

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<LoginResponse>(jsonString);
                _jwtToken = result.Token;
                ActualizarAuthHeader();
                return null;
            }
            return $"Error: {response.ReasonPhrase}";
        }

        public async Task<string> RegistrarAsync(string username, string email, string password)
        {
            var request = new RegisterRequest { Username = username, Email = email, Password = password };
            var response = await _httpClient.PostAsync("auth/registrar", CrearContenidoJson(request));

            return response.IsSuccessStatusCode ? null : $"Error al registrar: {response.ReasonPhrase}";
        }

        public void Logout()
        {
            _jwtToken = string.Empty;
            _httpClient.DefaultRequestHeaders.Authorization = null;
            _httpClient.PostAsync("auth/logout", null);
        }

        public async Task<List<Producto>> ObtenerProductosAsync()
        {
            var response = await _httpClient.GetAsync("productos");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Producto>>(json);
            }
            return new List<Producto>();
        }

        public async Task<bool> CrearProductoAsync(Producto p)
        {
            var response = await _httpClient.PostAsync("productos", CrearContenidoJson(p));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarProductoAsync(Producto p)
        {
            var response = await _httpClient.PutAsync($"productos/{p.Id}", CrearContenidoJson(p));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarProductoAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"productos/{id}");
            return response.IsSuccessStatusCode;
        }
        public async Task<List<Categoria>> ObtenerCategoriasAsync()
        {
            var response = await _httpClient.GetAsync("categorias");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Categoria>>(json);
            }
            return new List<Categoria>();
        }

        public async Task<bool> CrearCategoriaAsync(Categoria c)
        {
            var response = await _httpClient.PostAsync("categorias", CrearContenidoJson(c));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarCategoriaAsync(Categoria c)
        {
            var response = await _httpClient.PutAsync($"categorias/{c.Id}", CrearContenidoJson(c));
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarCategoriaAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"categorias/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EnviarPdfEmailAsync(string emailDestino)
        {
            var request = new EmailRequest { Email = emailDestino };
            var response = await _httpClient.PostAsync("productos/enviar-email", CrearContenidoJson(request));
            return response.IsSuccessStatusCode;
        }
    }
}