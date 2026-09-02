using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC.Models;
using BibliotecaMVC.Services;

namespace BibliotecaMVC.Controllers
{
    public class AutoresController : Controller
    {
        private readonly IAutorService _autorService;

        // Inyección de dependencias por constructor (Principio de Inversión de Dependencias)
        public AutoresController(IAutorService autorService)
        {
            _autorService = autorService;
        }

        // Listado de Autores
        public IActionResult Index()
        {
            var autores = _autorService.ObtenerTodos();
            return View(autores);
        }

        // Detalle del Autor
        public IActionResult Detalles(int id)
        {
            var autor = _autorService.ObtenerPorId(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // GET de autor para editar
        public IActionResult Editar(int id)
        {
            var autor = _autorService.ObtenerPorId(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // POST de autor para editar
        [HttpPost]
        public IActionResult Editar(Autor autorActualizado)
        {
            var autorExistente = _autorService.ObtenerPorId(autorActualizado.Id);
            if (autorExistente == null)
            {
                return NotFound();
            }

            _autorService.Actualizar(autorActualizado);
            return RedirectToAction("Index");
        }

        // GET para eliminar autor
        public IActionResult Eliminar(int id)
        {
            _autorService.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}