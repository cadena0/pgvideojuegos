using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaVideojuegos.Data;
using PaginaVideojuegos.Models;

namespace PaginaVideojuegos.Controllers;

public class GalleryController : Controller
{
    private readonly MysqlDbContext _context;

    public GalleryController(MysqlDbContext context)
    {
        _context = context;
    }

    private IActionResult? RequireLogin()
    {
        if (HttpContext.Session.GetInt32("UserId") == null)
            return RedirectToAction("Login", "User");
        return null;
    }

    // GET: /Gallery
    public async Task<IActionResult> Index()
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;

        var entries = await _context.GameEntries
            .Include(g => g.User)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();

        return View(entries);
    }

    // POST: /Gallery/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GameEntryViewModel model)
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;

        if (!ModelState.IsValid)
        {
            // Return errors as JSON for the modal
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    x => x.Key,
                    x => x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return Json(new { success = false, errors });
        }

        int userId = HttpContext.Session.GetInt32("UserId")!.Value;

        var entry = new GameEntry
        {
            Title = model.Title,
            ImageUrl = model.ImageUrl,
            Description = model.Description,
            UserId = userId
        };

        _context.GameEntries.Add(entry);
        await _context.SaveChangesAsync();

        return Json(new { success = true });
    }

    // POST: /Gallery/Delete
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return Json(new { success = false, message = "No autorizado" });

        var entry = await _context.GameEntries.FindAsync(id);
        if (entry == null) return Json(new { success = false, message = "No encontrado" });

        if (entry.UserId != userId)
            return Json(new { success = false, message = "No puedes eliminar esto" });

        _context.GameEntries.Remove(entry);
        await _context.SaveChangesAsync();

        return Json(new { success = true });
    }
}
