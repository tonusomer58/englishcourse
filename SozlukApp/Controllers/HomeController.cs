using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SozlukApp.Models;

namespace SozlukApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SozlukApp.Data.SozlukContext _context;

    public HomeController(ILogger<HomeController> logger, SozlukApp.Data.SozlukContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Dictionary()
    {
        var words = await _context.Words
                            .Where(w => w.Status == SozlukApp.Models.WordStatus.Approved)
                            .OrderByDescending(w => w.Id)
                            .ToListAsync();
        return View(words);
    }

    public IActionResult LevelTest()
    {
        return View();
    }

    public async Task<IActionResult> MakeMeAdmin()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username)) return Content("Lütfen önce giriş yapın.");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user != null)
        {
            user.Role = "Admin";
            await _context.SaveChangesAsync();
            return Content($"Başarılı! {username} artık Admin. Lütfen Çıkış yapıp tekrar Giriş yapın.");
        }
        return Content("Kullanıcı bulunamadı.");
    }

    public IActionResult Courses()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
