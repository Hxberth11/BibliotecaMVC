using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC.Models;
using BibliotecaMVC.Respositories;

namespace BibliotecaMVC.Controllers
{
    public class LibrosController : Controller
    {
        private readonly IRepositorioLibro _repositorio;

        public LibrosController(IRepositorioLibro repositorio)
        {
            _repositorio = repositorio;
        }

        // GET: Libros
        public IActionResult Index()
        {
            var libros = _repositorio.ObtenerLibros();
            return View(libros);
        }

        // GET: Libros/Details/5
        public IActionResult Details(int id)
        {
            var libro = _repositorio.ObtenerPorId(id);
            if (libro == null) return NotFound();

            return View(libro);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(libro.ImagenUrl))
                {
                    libro.ImagenUrl = "default.jpg";
                }

                _repositorio.AgregarLibro(libro);
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        // GET: Libros/Edit/5
        public IActionResult Edit(int id)
        {
            var libro = _repositorio.ObtenerPorId(id);
            if (libro == null) return NotFound();

            return View(libro);
        }

        // POST: Libros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Libro libroModificado)
        {
            if (id != libroModificado.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _repositorio.ActualizarLibro(libroModificado);
                return RedirectToAction(nameof(Index));
            }
            return View(libroModificado);
        }

        // GET/POST: Libros/Delete/5
        public IActionResult Delete(int id)
        {
            var libro = _repositorio.ObtenerPorId(id);
            if (libro != null)
            {
                _repositorio.EliminarLibro(libro);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
