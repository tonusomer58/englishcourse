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
            // Bekleyen kelimeleri listele, oluşturulma zamanı vs yok ama ters sırayla veya isme göre.
            // Önce bekleyenler, sonra onaylılar şeklinde de olabilir ama sadece bekleyenler yönetimi daha kolay.
            // Requirement says: "Bekleyen kelimeleri listeleyebilsin."
            var words = await _context.Words
                                .Include(w => w.CreatedByUser)
                                .OrderBy(w => w.Status) // Pending (0) first
                                .ThenByDescending(w => w.Id)
                                .ToListAsync();
            return View(words);
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
