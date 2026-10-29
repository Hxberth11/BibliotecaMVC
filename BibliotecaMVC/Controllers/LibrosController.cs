using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaMVC.Data;
using BibliotecaMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging; 

namespace BibliotecaMVC.Controllers
{
    [Authorize] // Requerir autenticación para acceder a libros
    public class LibrosController : Controller
    {
        private readonly BibliotecaContext _context;
        private readonly ILogger<LibrosController> _logger; // <-- 1. Declarar el logger

        // 3. Recibir el ILogger junto al contexto en el constructor
        public LibrosController(BibliotecaContext context, ILogger<LibrosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Libros (Mostrar el listado de libros - Accesible para cualquier usuario autenticado)
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Iniciando consulta para listar todos los libros."); // <-- Log de inicio

            var libros = await _context.Libros.ToListAsync();

            _logger.LogInformation("Se listaron {Cantidad} libros correctamente.", libros.Count); // <-- Log de éxito
            return View(libros);
        }

        // GET: Libros/Details/5 (Ver detalle - Accesible para cualquier usuario autenticado)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Intento de consulta de detalles con ID nulo.");
                return NotFound();
            }

            _logger.LogInformation("Consultando detalles del libro con ID: {LibroId}", id);

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.Id == id);

            if (libro == null)
            {
                _logger.LogWarning("Intento fallido: No se encontró ningún libro con el ID {LibroId}.", id); 
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libros/Create (Admin y Bibliotecario)
        [Authorize(Roles = "Admin, Bibliotecario")]
        public IActionResult Create()
        {
            _logger.LogInformation("Accediendo a la vista de creación de un nuevo libro.");
            return View();
        }

        // POST: Libros/Create (Admin y Bibliotecario)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Bibliotecario")]
        public async Task<IActionResult> Create(Libro libro)
        {
            _logger.LogInformation("Iniciando proceso de creación de un nuevo libro.");

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(libro.ImagenUrl))
                {
                    libro.ImagenUrl = "default.jpg";
                }

                try
                {
                    _context.Libros.Add(libro);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Libro creado exitosamente en la base de datos.");
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, "Ocurrió un error inesperado al intentar guardar un nuevo libro."); 
                    ModelState.AddModelError("", "Ocurrió un error al guardar el registro.");
                }
            }

            return View(libro);
        }

        // GET: Libros/Editar/5 (Admin y Bibliotecario)
        [Authorize(Roles = "Admin, Bibliotecario")]
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Intento de edición con ID nulo.");
                return NotFound();
            }

            _logger.LogInformation("Consultando libro con ID: {LibroId} para edición.", id);

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                _logger.LogWarning("Intento fallido: No se encontró el libro con ID {LibroId} para editar.", id);
                return NotFound();
            }
            return View(libro);
        }

        // POST: Libros/Editar/5 (Admin y Bibliotecario)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Bibliotecario")]
        public async Task<IActionResult> Editar(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                _logger.LogWarning("Conflicto de IDs en edición: ID de ruta ({RouteId}) no coincide con ID de modelo ({ModelId}).", id, libro.Id);
                return BadRequest();
            }

            _logger.LogInformation("Actualizando información del libro con ID: {LibroId}", id);

            if (string.IsNullOrEmpty(libro.ImagenUrl))
            {
                libro.ImagenUrl = "default.jpg";
            }

            try
            {
                _context.Update(libro);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Libro con ID {LibroId} actualizado correctamente.", id);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!LibroExists(libro.Id))
                {
                    _logger.LogWarning("Error de concurrencia: El libro con ID {LibroId} ya no existe.", libro.Id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Error de concurrencia crítico al actualizar el libro con ID {LibroId}.", libro.Id);
                    throw;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado al actualizar el libro con ID {LibroId}.", id);
                throw;
            }
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.Id == id);
        }

        // POST: Libros/Eliminar/5 (Admin y Bibliotecario)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Bibliotecario")]
        public async Task<IActionResult> Eliminar(int id)
        {
            _logger.LogInformation("Iniciando proceso de eliminación para el libro con ID: {LibroId}", id);

            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                try
                {
                    _context.Libros.Remove(libro);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Libro con ID {LibroId} eliminado exitosamente.", id);
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, "Error al intentar eliminar el libro con ID {LibroId}.", id);
                }
            }
            else
            {
                _logger.LogWarning("Intento de eliminar un libro inexistente con ID {LibroId}.", id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}