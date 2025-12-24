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

        [Authorize] // Allow any authenticated user
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

                // If Admin adds it, can be auto-approved? 
                // For now, adhere to plan: "User -> Pending". 
                // If Admin wants to approve, they can do it in Panel. 
                // But typically Admin adds = Approved.
                // Let's check role.
                if (User.IsInRole("Admin"))
                {
                     model.Status = WordStatus.Approved;
                     TempData["Message"] = "Kelime doğrulandı ve eklendi.";
                }
                else
                {
                     model.Status = WordStatus.Pending;
                     TempData["Message"] = "Kelime öneriniz alındı, onay için yöneticiye gönderildi.";
                }

                _context.Words.Add(model);
                await _context.SaveChangesAsync();
                
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Panel", "Admin");
                }
                else
                {
                    // User stays on page or goes to Dictionary? 
                    // Stays on page to add more is often better, or redirect to Home/Dictionary?
                    // Let's redirect to Dictionary with the message visible.
                    return RedirectToAction("Dictionary", "Home");
                }
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
