using HospitalSanJoseModel;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace HospitalSanJoseModel.Functions
{
    public class APIServices
    {

        protected readonly int Timeout = 30;
        private readonly string Url = "https://localhost:7256/";
        private readonly HttpStatusCode[] ErrorCodes = {  HttpStatusCode.InternalServerError };
       
        protected  async Task<T> Get<T>(string route = "")
        {

            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(Timeout)
            };
            var response = await httpClient.GetAsync(Url + route);
            if (ErrorCodes.Contains(response.StatusCode))
            {
                throw new Exception(
                    $"Code:${response.StatusCode}\nMessage:${response.Content}"
                );
                throw new Exception(response.StatusCode.ToString());
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {

            }
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        protected async Task<T?> Post<T>(T content, string route = "")
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

            var response = await httpClient.PostAsync(Url + route, jsonContent);
            if (ErrorCodes.Contains(response.StatusCode))
            {
                throw new Exception(response.StatusCode.ToString());
            }
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            
        }
        protected async Task<T?> Put<T>(T content, string path = "")
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

            var response = await httpClient.PutAsync(Url + path, jsonContent);
            if (ErrorCodes.Contains(response.StatusCode))
            {
                throw new Exception(
                    $"Code: {response.StatusCode.ToString()}"
                );
                throw new Exception(response.StatusCode.ToString());
            }
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        protected async Task Delete(string path = "")
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };
            HttpClient httpClient = new(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(Timeout)
            };

            var response = await httpClient.DeleteAsync(Url + path);
            if (ErrorCodes.Contains(response.StatusCode))
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
    }
}
