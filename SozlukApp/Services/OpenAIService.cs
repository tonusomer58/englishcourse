using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SozlukApp.Services
{
    public class OpenAIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OpenAIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> TranslateAsync(string text)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_OPENAI_API_KEY_HERE") 
                return "⚠️ API Key Eksik! (appsettings.json dosyasına anahtarınızı giriniz)";

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful translator. Translate the given English text to Turkish. Only provide the translation." },
                    new { role = "user", content = text }
                }
            };

            return await CallOpenAI(requestBody, apiKey);
        }

        public async Task<string> GenerateExampleSentenceAsync(string word)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_OPENAI_API_KEY_HERE")
                 return "⚠️ API Key Eksik! (appsettings.json dosyasına anahtarınızı giriniz)";

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful English tutor. Generate a simple English example sentence using the given word." },
                    new { role = "user", content = $"Use the word '{word}' in a simple sentence." }
                }
            };

            return await CallOpenAI(requestBody, apiKey);
        }

        private async Task<string> CallOpenAI(object requestBody, string apiKey)
        {
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            try
            {
                var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(responseString);
                    var result = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                    return result?.Trim() ?? "No result";
                }
                else
                {
                     return $"Error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return $"Exception: {ex.Message}";
            }
        }
    }
}
