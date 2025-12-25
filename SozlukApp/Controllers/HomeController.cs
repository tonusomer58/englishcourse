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

    [Route("Dictionary")]
    public async Task<IActionResult> Dictionary(string searchString)
    {
        var wordsQuery = _context.Words
                            .Where(w => w.Status == SozlukApp.Models.WordStatus.Approved);

        if (!string.IsNullOrEmpty(searchString))
        {
            wordsQuery = wordsQuery.Where(w => w.Turkish.Contains(searchString) || w.English.Contains(searchString));
        }

        var words = await wordsQuery
                            .OrderByDescending(w => w.Id)
                            .ToListAsync();
        
        ViewData["CurrentFilter"] = searchString;
        return View(words);
    }

    [Route("LevelTest")]
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

    public async Task<IActionResult> FixAdmin()
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == "tonusomer");
        if (user != null)
        {
            user.Role = "Admin";
            await _context.SaveChangesAsync();
            return Content("Success: 'tonusomer' role set to Admin. Please Logout and Login again.");
        }
        return Content("Error: User 'tonusomer' not found.");
    }

    public async Task<IActionResult> DebugInfo()
    {
        var username = "tonusomer";
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
        var dbRole = user?.Role ?? "User Not Found";
        
        var claims = User.Claims.Select(c => $"{c.Type}: {c.Value}").ToList();
        
        return Json(new { DbRole = dbRole, CookieClaims = claims });
    }

    [Route("Courses")]
    public IActionResult Courses()
    {
        return View();
    }

    [Route("About")]
    public IActionResult About()
    {
        return View();
    }

    [Route("Contact")]
    public IActionResult Contact()
    {
        return View();
    }

    [Route("Privacy")]
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
