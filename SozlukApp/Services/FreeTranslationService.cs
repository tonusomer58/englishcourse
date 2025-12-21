using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SozlukApp.Services
{
    public class FreeTranslationService : IAIService
    {
        private readonly HttpClient _httpClient;

        public FreeTranslationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> TranslateAsync(string text)
        {
            // UI sends Turkish word, expects English translation.
            string fromLanguage = "tr";
            string toLanguage = "en";
            
            try
            {
                // MyMemory API (More stable free service)
                string url = $"https://api.mymemory.translated.net/get?q={Uri.EscapeDataString(text)}&langpair={fromLanguage}|{toLanguage}";
                var response = await _httpClient.GetStringAsync(url);
                
                using var doc = JsonDocument.Parse(response);
                var translatedText = doc.RootElement
                                        .GetProperty("responseData")
                                        .GetProperty("translatedText")
                                        .GetString();

                return translatedText ?? text; // Return original if null
            }
            catch
            {
                return text; // Return original on error
            }
        }

        public async Task<string> GenerateExampleSentenceAsync(string word)
        {
            try
            {
                // Try DictionaryAPI first
                var response = await _httpClient.GetStringAsync($"https://api.dictionaryapi.dev/api/v2/entries/en/{Uri.EscapeDataString(word)}");
                using var doc = JsonDocument.Parse(response);
                
                string bestExample = null;

                // Deep search for example sentence
                foreach (var element in doc.RootElement.EnumerateArray())
                {
                    if (element.TryGetProperty("meanings", out var meanings))
                    {
                        foreach (var meaning in meanings.EnumerateArray())
                        {
                            if (meaning.TryGetProperty("definitions", out var definitions))
                            {
                                foreach (var def in definitions.EnumerateArray())
                                {
                                    if (def.TryGetProperty("example", out var exampleJson))
                                    {
                                        string currentExample = exampleJson.GetString();
                                        // Filter out short fragments like "to table fines"
                                        if (!string.IsNullOrWhiteSpace(currentExample) && currentExample.Length > 15) 
                                        {
                                            // Pick the longest example we find, usually better quality
                                            if (bestExample == null || currentExample.Length > bestExample.Length)
                                            {
                                                bestExample = currentExample;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (bestExample != null)
                {
                    return bestExample;
                }
            }
            catch
            {
                // Ignore errors (API not found etc) and fall through to fallback
            }

            // Fallback sentence if no suitable example found in API
            return $"This is a sample sentence using the word '{word}'.";
        }
    }
}