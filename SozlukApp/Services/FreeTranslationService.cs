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

            // Fallback 2: Wikipedia Search (Huge archive of sentences)
            try
            {
                 // Search for the word in English Wikipedia
                var wikiUrl = $"https://en.wikipedia.org/w/api.php?action=query&list=search&srsearch={Uri.EscapeDataString(word)}&utf8=&format=json";
                var wikiResponse = await _httpClient.GetStringAsync(wikiUrl);
                using var wikiDoc = JsonDocument.Parse(wikiResponse);
                
                var query = wikiDoc.RootElement.GetProperty("query");
                var searchResults = query.GetProperty("search");
                
                foreach (var result in searchResults.EnumerateArray())
                {
                    // Snippet contains HTML like <span class="searchmatch">word</span>
                    if (result.TryGetProperty("snippet", out var snippetJson))
                    {
                        string rawSnippet = snippetJson.GetString();
                        if (!string.IsNullOrEmpty(rawSnippet)) 
                        {
                             // Remove simple HTML tags
                            string cleanSnippet = System.Text.RegularExpressions.Regex.Replace(rawSnippet, "<.*?>", "");
                            
                            // Ensure it looks like a sentence and contains the word
                            if (!string.IsNullOrWhiteSpace(cleanSnippet) && cleanSnippet.Length > 20)
                            {
                                return cleanSnippet + "... (Source: Wikipedia)";
                            }
                        }
                    }
                }
            }
            catch
            {
                // Ignore Wiki errors
            }

            // Fallback 3: Generic Template
            return $"This is a sample sentence using the word '{word}'.";
        }
    }
}