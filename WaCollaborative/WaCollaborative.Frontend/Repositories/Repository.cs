#region Using

using System.Text;
using System.Text.Json;
using WaCollaborative.Frontend.Helpers;

#endregion Using

namespace WaCollaborative.Frontend.Repositories
{

    /// <summary>
    /// The Class Repository
    /// </summary>

    public class Repository : IRepository
    {

        #region Attributes

        private readonly HttpClient _httpClient;

        #endregion Attributes

        #region Constructor

        public Repository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #endregion Constructor

        #region Methods

        private JsonSerializerOptions _jsonDefaultOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        private async Task<T> UnserializeAnswer<T>(HttpResponseMessage responseHttp)
        {
            var response = await responseHttp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(response, _jsonDefaultOptions)!;
        }
        public async Task<HttpResponseWrapper<string>> GetExportAsync(string url)
        {
            var responseHTTP = await _httpClient.GetAsync(url);

            if (responseHTTP.IsSuccessStatusCode)
            {
                var jsonResponse = await responseHTTP.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<ExcelLinkResponse>(jsonResponse, _jsonDefaultOptions);

                if (responseObject != null)
                {
                    return new HttpResponseWrapper<string>(responseObject.ExcelLink, false, responseHTTP);
                }                
            }

            return new HttpResponseWrapper<string>(null, true, responseHTTP);
        }
        public async Task<HttpResponseWrapper<object>> GetAsync(string url)
        {
            var responseHTTP = await _httpClient.GetAsync(url);
            return new HttpResponseWrapper<object>(null, !responseHTTP.IsSuccessStatusCode, responseHTTP);

        }

        public async Task<HttpResponseWrapper<T>> GetAsync<T>(string url)
        {
            var responseHttp = await _httpClient.GetAsync(url);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<T>(responseHttp);
                return new HttpResponseWrapper<T>(response, false, responseHttp);
            }

            return new HttpResponseWrapper<T>(default, true, responseHttp);
        }

        public async Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContet = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, messageContet);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TResponse>> PostAsync<T, TResponse>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContet = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, messageContet);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<TResponse>(responseHttp);
                return new HttpResponseWrapper<TResponse>(response, false, responseHttp);
            }

            return new HttpResponseWrapper<TResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContet = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PutAsync(url, messageContet);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TResponse>> PutAsync<T, TResponse>(string url, T model)
        {
            var messageJSON = JsonSerializer.Serialize(model);
            var messageContet = new StringContent(messageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PutAsync(url, messageContet);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<TResponse>(responseHttp);
                return new HttpResponseWrapper<TResponse>(response, false, responseHttp);
            }

            return new HttpResponseWrapper<TResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<object>> DeleteAsync(string url)
        {
            var responseHttp = await _httpClient.DeleteAsync(url);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }
        

        #endregion Methods

    }
}