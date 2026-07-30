using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Controllers
{
    public class LibrosController : Controller
    {
        // Lista de los libros (es prueba porque no se si lo de las imagenes va a funcionar)
        private static List<Libro> _libros = new List<Libro>
        {
            new Libro { Id = 1, Titulo = "Cien años de soledad", Autor = "Gabriel García Márquez", Genero = "Realismo Mágico", AnioPublicacion = 1967, ImagenUrl = "cien_anios.webp" },
            new Libro { Id = 2, Titulo = "Ficciones", Autor = "Jorge Luis Borges", Genero = "Ficción", AnioPublicacion = 1944, ImagenUrl = "ficciones.jpg" }
        };

        // El index de libros
        public IActionResult Index()
        {
            return View(_libros);
        }

        // detalles de un libro
        public IActionResult Details(int id)
        {
            var libro = _libros.FirstOrDefault(l => l.Id == id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        // El get de libros
        public IActionResult Create()
        {
            return View();
        }

        // El post de libros
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                libro.Id = _libros.Any() ? _libros.Max(l => l.Id) + 1 : 1;
                if (string.IsNullOrEmpty(libro.ImagenUrl))
                {
                    libro.ImagenUrl = "default.jpg"; // Imagen por defecto(prueba)
                }
                _libros.Add(libro);
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        // Editar el libro (GET)
        public IActionResult Edit(int id)
        {
            var libro = _libros.FirstOrDefault(l => l.Id == id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        // Editar el Libro (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Libro libroModificado)
        {
            if (id != libroModificado.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var libro = _libros.FirstOrDefault(l => l.Id == id);
                if (libro != null)
                {
                    libro.Titulo = libroModificado.Titulo;
                    libro.Autor = libroModificado.Autor;
                    libro.Genero = libroModificado.Genero;
                    libro.AnioPublicacion = libroModificado.AnioPublicacion;
                    libro.ImagenUrl = libroModificado.ImagenUrl;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(libroModificado);
        }

        // Eliminar Libro
        public IActionResult Delete(int id)
        {
            var libro = _libros.FirstOrDefault(l => l.Id == id);
            if (libro != null)
            {
                _libros.Remove(libro);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}