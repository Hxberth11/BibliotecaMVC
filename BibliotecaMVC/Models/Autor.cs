using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BibliotecaMVC.Models
{
    public class Autor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        [StringLength(100)]
        public string Apellido { get; set; }
        [StringLength(100)]
        public string Nacionalidad { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }
        public bool Activo { get; set; }
    }
}
