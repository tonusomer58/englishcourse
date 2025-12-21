using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SozlukApp.Data;
using SozlukApp.Models;
using System.Security.Claims;

namespace SozlukApp.Controllers
{
    [Authorize]

    public class WordController : Controller
    {
        private readonly SozlukContext _context;
        private readonly SozlukApp.Services.IAIService _aiService;

        public WordController(SozlukContext context, SozlukApp.Services.IAIService aiService)
        {
            _context = context;
            _aiService = aiService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Word model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue("UserId");
                if (userId != null)
                {
                    model.CreatedByUserId = int.Parse(userId);
                }

                model.Status = WordStatus.Pending;
                _context.Words.Add(model);
                await _context.SaveChangesAsync();
                
                TempData["Message"] = "Kelime başarıyla eklendi.";
                return RedirectToAction("Panel", "Admin");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetTranslationSuggestion(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return Json(new { suggestion = "" });

            var suggestion = await _aiService.TranslateAsync(text);
            return Json(new { suggestion });
        }

        [HttpGet]
        public async Task<IActionResult> GetExampleSentence(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return Json(new { sentence = "" });

            var sentence = await _aiService.GenerateExampleSentenceAsync(word);
            return Json(new { sentence });
        }
    }
}
