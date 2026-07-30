using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BibliotecaMVC.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")] // Preferi ponerle mensajes Ingeniero
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio")]
        public string Autor { get; set; } = string.Empty;

        public string Genero { get; set; } = string.Empty;

        public int AnioPublicacion { get; set; }

        // la imagen guardada en wwwroot/images
        public string ImagenUrl { get; set; } = string.Empty;
    }
}