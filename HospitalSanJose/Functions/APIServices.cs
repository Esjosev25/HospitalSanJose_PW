
using Microsoft.AspNetCore.Http;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace HospitalSanJose.Functions
{
    public class APIServices
    {

        protected readonly int Timeout = 30;
        private readonly string Url = "https://localhost:7256/";
        private readonly HttpStatusCode[] ErrorCodes = { HttpStatusCode.InternalServerError, HttpStatusCode.Unauthorized};
        private readonly IHttpContextAccessor _accessor;

        public APIServices(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        protected async Task<T> Get<T>(string route = "")
        {

            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(Timeout)
            };
            
            var _httpContext = _accessor.HttpContext;
            var token = _httpContext!.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.GetAsync(Url + route);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {

                _accessor.HttpContext.Response.StatusCode = 401;
                _accessor.HttpContext.Response.Redirect("Error/401");
                return default(T);
            }

            if (ErrorCodes.Contains(response.StatusCode))
            {
                throw new Exception(
                    $"Code:${response.StatusCode}\nMessage:${response.Content}"
                );
                throw new Exception(response.StatusCode.ToString());
            }
           
            return JsonConvert.DeserializeObject<T?>(await response.Content.ReadAsStringAsync());
        }


        protected async Task<T?> Post<T>(object? content, string route = "")
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(Timeout)
            };

            var json = JsonConvert.SerializeObject(content);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var _httpContext = _accessor.HttpContext;
            var token = _httpContext!.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.PostAsync(Url + route, jsonContent);
            if (ErrorCodes.Contains(response.StatusCode))
            {
                throw new Exception(response.StatusCode.ToString());
            }
          
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

        }
        protected async Task<T?> Put<T>(T content, string route = "")
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(Timeout)
            };

            var json = JsonConvert.SerializeObject(content);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var _httpContext = _accessor.HttpContext;
            var token = _httpContext!.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.PutAsync(Url + route, jsonContent);
            if (ErrorCodes.Contains(response.StatusCode))
            {
                throw new Exception(
                    $"Code: {response.StatusCode}"
                );
            
            }
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        protected async Task Patch(string route = "")
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(Timeout)
            };

            var _httpContext = _accessor.HttpContext;
            var token = _httpContext!.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.PatchAsync(Url + route, null);
            if (ErrorCodes.Contains(response.StatusCode))
            {
                throw new Exception(
                    $"Code: {response.StatusCode.ToString()}"
                );
                throw new Exception(response.StatusCode.ToString());
            }

        }


        protected async Task Delete(string route = "")
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(Timeout)
            };

            var _httpContext = _accessor.HttpContext;
            var token = _httpContext!.Session.GetString("Token");
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.DeleteAsync(Url + route);
            if (ErrorCodes.Contains(response.StatusCode))
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
    }
}
