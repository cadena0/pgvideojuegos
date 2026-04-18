using System.ComponentModel.DataAnnotations;

namespace PaginaVideojuegos.Models;

public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de usuario es requerido")]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "Email no válido")]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [StringLength(255)]
    public string Password { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<GameEntry> GameEntries { get; set; } = new List<GameEntry>();
}
