using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ok.Service
{
    public class LlmService
    {
        private readonly string _apiKey;
        private readonly string _endpoint;
        private readonly HttpClient _httpClient;

        public LlmService(string apiKey, string endpoint, HttpClient httpClient)
        {
            _apiKey = apiKey;
            _endpoint = endpoint;
            _httpClient = httpClient;
        }

        public async Task<string> GenerateAsync(string prompt)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var body = new
            {
                prompt = prompt,
                max_tokens = 100
            };

            request.Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return json;
        }
    }
}
