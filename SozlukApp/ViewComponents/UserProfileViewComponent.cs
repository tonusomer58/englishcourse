using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SozlukApp.Data;
using SozlukApp.Models;
using System.Security.Claims;

namespace SozlukApp.ViewComponents
{
    public class UserProfileViewComponent : ViewComponent
    {
        private readonly SozlukContext _context;

        public UserProfileViewComponent(SozlukContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userIdStr = HttpContext.User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userIdStr))
            {
                return Content("");
            }

            int userId = int.Parse(userIdStr);
            var results = await _context.TestResults
                                .Where(t => t.UserId == userId)
                                .OrderByDescending(t => t.DateTaken)
                                .ToListAsync();

            var viewModel = new UserProfileViewModel
            {
                Username = HttpContext.User.Identity?.Name ?? "",
                TotalTests = results.Count,
                LastLevel = results.FirstOrDefault()?.Level ?? "Belirsiz",
                AverageScore = results.Any() ? Math.Round(results.Average(r => (double)r.CorrectCount / r.TotalQuestions * 100)) : 0
            };

            return View(viewModel);
        }
    }

    public class UserProfileViewModel
    {
        public string Username { get; set; }
        public int TotalTests { get; set; }
        public string LastLevel { get; set; }
        public double AverageScore { get; set; }
    }
}
