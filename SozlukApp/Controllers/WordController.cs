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

        public WordController(SozlukContext context)
        {
            _context = context;
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
        public IActionResult GetTranslationSuggestion(string text)
        {
             // AI Disabled due to build error rollback
             return Json(new { suggestion = "" });
        }

        [HttpGet]
        public IActionResult GetExampleSentence(string word)
        {
             // AI Disabled due to build error rollback
             return Json(new { sentence = "" });
        }
    }
}
