using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaMVC.Data;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Controllers
{
    public class AutoresController : Controller
    {
        // 1. Inyectamos la base de datos (BibliotecaContext) directamente
        private readonly BibliotecaContext _context;

        public AutoresController(BibliotecaContext context)
        {
            _context = context;
        }

        // 2. Listado de Autores desde SQL Server
        public async Task<IActionResult> Index()
        {
            var autores = await _context.Autores.ToListAsync();
            return View(autores);
        }

        // 3. Detalle del Autor
        public async Task<IActionResult> Details(int id)
        {
            var autor = await _context.Autores.FirstOrDefaultAsync(a => a.Id == id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // 4. GET: Vista para crear autor
        public IActionResult Create()
        {
            return View();
        }

        // 5. POST: Guardar nuevo autor en la base de datos (Tal como lo tiene el ingeniero)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Autor autor)
        {
            if (!ModelState.IsValid)
            {
                return View(autor);
            }

            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 6. GET: Vista para editar autor
        public async Task<IActionResult> Editar(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // 7. POST: Guardar cambios de edición
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id,Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return View(autor);
            }

            var existeAutor = await _context.Autores.AnyAsync(a => a.Id == id);
            if (!existeAutor)
            {
                return NotFound();
            }

            _context.Update(autor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // 8. POST: Eliminar autor
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor == null) // Correcto: si NO existe, devuelve NotFound
            {
                return NotFound();
            }

            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}