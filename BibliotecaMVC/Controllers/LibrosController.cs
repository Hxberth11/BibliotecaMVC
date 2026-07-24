using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Controllers
{
    public class LibrosController : Controller
    {
        public IActionResult Index()
        {
            List<Libro> Libros = new List<Libro>
            {
                new Libro { Id = 1, Titulo = "Cien Años de Soledad", Autor = "Gabriel García Márquez", Categoria = "Novela", Precio = 19.99m, Disponible = true },
                new Libro { Id = 2, Titulo = "La casa de los espíritus", Autor = "Isabel Allende", Categoria = "Novela", Precio = 24.99m, Disponible = true },
                new Libro { Id = 3, Titulo = "Ficciones", Autor = "Jorge Luis Borges", Categoria = "Cuento", Precio = 15.99m, Disponible = false },
                new Libro { Id = 4, Titulo = "La ciudad y los perros", Autor = "Mario Vargas Llosa", Categoria = "Novela", Precio = 22.99m, Disponible = true },
                new Libro { Id = 5, Titulo = "Rayuela", Autor = "Julio Cortázar", Categoria = "Novela", Precio = 20.99m, Disponible = false }
            };
            return View(Libros);
        }
    }
}
