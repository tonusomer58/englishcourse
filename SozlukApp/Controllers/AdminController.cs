using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SozlukApp.Data;
using SozlukApp.Models;

namespace SozlukApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly SozlukContext _context;

        public AdminController(SozlukContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Panel()
        {
            // Bekleyen kelimeleri listele
            var words = await _context.Words
                                .Include(w => w.CreatedByUser)
                                .OrderBy(w => w.Status) // Pending (0) first
                                .ThenByDescending(w => w.Id)
                                .ToListAsync();

            // Test sonuçlarını listele (Leaderboard: En yüksek başarı oranı en üstte)
            var testResults = await _context.TestResults
                                .Include(tr => tr.User)
                                .OrderByDescending(tr => (double)tr.CorrectCount / tr.TotalQuestions)
                                .ThenByDescending(tr => tr.DateTaken)
                                .ToListAsync();

            var model = new AdminPanelViewModel
            {
                Words = words,
                TestResults = testResults,
                TotalTestsTaken = testResults.Count,
                AverageScore = testResults.Any() ? Math.Round(testResults.Average(tr => (double)tr.CorrectCount / tr.TotalQuestions * 100), 1) : 0,
                TotalUsersTested = testResults.Select(tr => tr.UserId).Distinct().Count()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var word = await _context.Words.FindAsync(id);
            if (word != null)
            {
                word.Status = WordStatus.Approved;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Panel");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var word = await _context.Words.FindAsync(id);
            if (word != null)
            {
                word.Status = WordStatus.Rejected; // Instead of deleting, we can mark as Rejected or Delete. 
                // Requirement says: "Kelimeleri onaylama / reddetme / silme"
                // Let's implement Delete separate or use Reject as soft-delete logic. 
                // Let's stick to Status change for history, and Delete for actual deletion.
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Panel");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var word = await _context.Words.FindAsync(id);
            if (word != null)
            {
                _context.Words.Remove(word);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Panel");
        }
    }
}
