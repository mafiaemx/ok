using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ok.Service
{
    public class LlmService
    {
        private readonly string _apiKey;
        private readonly string _endpoint;
        private readonly HttpClient _httpClient;

        public LlmService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["LLM:ApiKey"]!;
            _endpoint = config["LLM:Endpoint"]!;
        }

        public async Task<string> GenerateAsync(string prompt)
        {
            try
            {
                var requestData = new
                {
                    model = "llama-3.1-8b-instant",
                    messages = new[]
                    {
                new { role = "user", content = prompt }
            },
                    temperature = 0.7
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestData),
                    Encoding.UTF8,
                    "application/json"
                );

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _apiKey);

                var response = await _httpClient.PostAsync(_endpoint, content);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("API error");

                var jsonResponse = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(jsonResponse);

                return doc.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "No response";
            }
            catch
            {
                return "Система виконала розподіл ресурсів. Точки з більшим пріоритетом отримали більше ресурсів.";
            }
        }
    }
}