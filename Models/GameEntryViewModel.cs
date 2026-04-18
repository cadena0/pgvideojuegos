using System.ComponentModel.DataAnnotations;

namespace PaginaVideojuegos.Models;

public class GameEntryViewModel
{
    [Required(ErrorMessage = "El título es requerido")]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "La URL de la imagen es requerida")]
    [Url(ErrorMessage = "Debe ser una URL válida (ej: https://...)")]
    [StringLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es requerida")]
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;
}
