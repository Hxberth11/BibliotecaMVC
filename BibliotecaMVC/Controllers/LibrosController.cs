using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaMVC.Data;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Controllers
{
    public class LibrosController : Controller
    {
        private readonly BibliotecaContext _context;

        public LibrosController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: Libros (Mostrar el listado de libros)
        public async Task<IActionResult> Index()
        {
            var libros = await _context.Libros.ToListAsync();
            return View(libros);
        }

        // GET: Libros/Details/5 (Ver detalle de un libro)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libros/Create (Mostrar formulario para agregar libro)
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libros/Create (Guardar el nuevo libro en SQL Server con EF Core)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(libro.ImagenUrl))
                {
                    libro.ImagenUrl = "default.jpg";
                }

                _context.Libros.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        // GET: Libros/Editar/5 (Carga el libro en el formulario)
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Busca el libro por ID utilizando EF Core
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            return View(libro);
        }

        // POST: Libros/Editar/5 (Guarda las modificaciones en la base de datos)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                return BadRequest();
            }

            // Asigna imagen por defecto si viene vacía
            if (string.IsNullOrEmpty(libro.ImagenUrl))
            {
                libro.ImagenUrl = "default.jpg";
            }

            // Forzamos actualización para evitar bloqueos por validación estricta de ModelState
            try
            {
                _context.Update(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LibroExists(libro.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.Id == id);
        }

        // POST: Libros/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}