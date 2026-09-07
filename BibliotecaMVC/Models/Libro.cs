using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BibliotecaMVC.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio")]
        public string Autor { get; set; } = string.Empty;

        public string Genero { get; set; } = string.Empty;

        public int AnioPublicacion { get; set; }

        // La imagen guardada en wwwroot/images
        public string ImagenUrl { get; set; } = string.Empty;

        // Nuevas propiedades
        [Range(0.01, 10000.00, ErrorMessage = "El precio debe ser un valor positivo mayor a 0")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        public bool Disponible { get; set; } = true;
    }
}