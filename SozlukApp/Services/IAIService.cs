using System.Threading.Tasks;

namespace SozlukApp.Services
{
    public interface IAIService
    {
        Task<string> TranslateAsync(string text);
        Task<string> GenerateExampleSentenceAsync(string word);
    }
}
