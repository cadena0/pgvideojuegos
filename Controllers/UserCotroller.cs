using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaginaVideojuegos.Data;
using PaginaVideojuegos.Models;

namespace PaginaVideojuegos.Controllers;

public class UserController : Controller
{
    private readonly MysqlDbContext _context;

    public UserController(MysqlDbContext context)
    {
        _context = context;
    }

    // GET: /User/Register
    public IActionResult Register()
    {
        if (HttpContext.Session.GetInt32("UserId") != null)
            return RedirectToAction("Index", "Gallery");
        return View();
    }

    // POST: /User/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        bool emailExists = await _context.Users.AnyAsync(u => u.Email == model.Email);
        if (emailExists)
        {
            ModelState.AddModelError("Email", "Este email ya está registrado");
            return View(model);
        }

        bool usernameExists = await _context.Users.AnyAsync(u => u.Username == model.Username);
        if (usernameExists)
        {
            ModelState.AddModelError("Username", "Este nombre de usuario ya está en uso");
            return View(model);
        }

        var user = new User
        {
            Username = model.Username,
            Email = model.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(model.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        TempData["Success"] = "¡Registro exitoso! Inicia sesión.";
        return RedirectToAction("Login");
    }

    // GET: /User/Login
    public IActionResult Login()
    {
        if (HttpContext.Session.GetInt32("UserId") != null)
            return RedirectToAction("Index", "Gallery");
        return View();
    }

    // POST: /User/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
        {
            ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
            return View(model);
        }

        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("Username", user.Username);

        return RedirectToAction("Index", "Gallery");
    }

    // GET: /User/Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
