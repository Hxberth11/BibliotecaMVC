using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using BibliotecaMVC.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging; 

namespace BibliotecaMVC.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly string _connectionString;
        private readonly ILogger<CategoriasController> _logger; // <-- 1. Declarar el logger

        // 3. Recibir el IConfiguration y el ILogger en el constructor
        public CategoriasController(IConfiguration configuration, ILogger<CategoriasController> logger)
        {
            _connectionString = configuration.GetConnectionString("BibliotecaDB");
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Iniciando consulta para listar todas las categorías.");
            var categorias = new List<Categoria>();

            try
            {
                using (var conexion = new SqlConnection(_connectionString))
                {
                    var sql = "SELECT Id, Nombre, Descripcion FROM Categorias";
                    using (var comando = new SqlCommand(sql, conexion))
                    {
                        conexion.Open();
                        using (var lector = comando.ExecuteReader())
                        {
                            while (lector.Read())
                            {
                                categorias.Add(new Categoria
                                {
                                    Id = lector.GetInt32(0),
                                    Nombre = lector.GetString(1),
                                    Descripcion = lector.IsDBNull(2) ? null : lector.GetString(2)
                                });
                            }
                        }
                    }
                }
                _logger.LogInformation("Se listaron {Cantidad} categorías exitosamente.", categorias.Count);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al intentar consultar el listado de categorías.");
            }

            return View(categorias);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            _logger.LogInformation("Accediendo a la vista de creación de una nueva categoría.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Categoria categoria)
        {
            _logger.LogInformation("Iniciando proceso para crear una nueva categoría.");

            if (categoria == null || string.IsNullOrEmpty(categoria.Nombre))
            {
                _logger.LogWarning("Intento fallido de creación: El nombre de la categoría es obligatorio o el objeto es nulo.");
                ModelState.AddModelError("Nombre", "El nombre de la categoría es obligatorio.");
                return View(categoria);
            }

            try
            {
                using (var conexion = new SqlConnection(_connectionString))
                {
                    var sql = "INSERT INTO Categorias (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)";

                    using (var comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                        comando.Parameters.AddWithValue("@Descripcion", (object)categoria.Descripcion ?? System.DBNull.Value);
                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }
                _logger.LogInformation("Categoría '{NombreCategoria}' creada exitosamente en la base de datos.", categoria.Nombre);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al intentar guardar la categoría '{NombreCategoria}'.", categoria.Nombre);
                ModelState.AddModelError("", "Ocurrió un error al guardar en la base de datos.");
                return View(categoria);
            }

            TempData["SuccessMessage"] = "Categoría creada exitosamente.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            _logger.LogInformation("Consultando categoría con ID: {CategoriaId} para edición.", id);
            Categoria categoria = null;

            try
            {
                using (var conexion = new SqlConnection(_connectionString))
                {
                    var sql = "SELECT Id, Nombre, Descripcion FROM Categorias WHERE Id = @Id";
                    using (var comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@Id", id);
                        conexion.Open();
                        using (var lector = comando.ExecuteReader())
                        {
                            if (lector.Read())
                            {
                                categoria = new Categoria
                                {
                                    Id = lector.GetInt32(0),
                                    Nombre = lector.GetString(1),
                                    Descripcion = lector.IsDBNull(2) ? null : lector.GetString(2)
                                };
                            }
                        }
                    }
                }

                if (categoria == null)
                {
                    _logger.LogWarning("Intento fallido: No se encontró la categoría con ID {CategoriaId} para editar.", id);
                    return NotFound();
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al consultar la categoría con ID {CategoriaId} para edición.", id);
            }

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Categoria categoria)
        {
            _logger.LogInformation("Iniciando actualización para la categoría con ID: {CategoriaId}", categoria?.Id);

            if (categoria == null || string.IsNullOrEmpty(categoria.Nombre))
            {
                _logger.LogWarning("Intento fallido de actualización: Nombre obligatorio o categoría nula.");
                ModelState.AddModelError("Nombre", "El nombre de la categoría es obligatorio.");
                return View(categoria);
            }

            try
            {
                using (var conexion = new SqlConnection(_connectionString))
                {
                    var sql = "UPDATE Categorias SET Nombre = @Nombre, Descripcion = @Descripcion WHERE Id = @Id";

                    using (var comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                        comando.Parameters.AddWithValue("@Descripcion", (object)categoria.Descripcion ?? System.DBNull.Value);
                        comando.Parameters.AddWithValue("@Id", categoria.Id);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }
                _logger.LogInformation("Categoría con ID {CategoriaId} actualizada correctamente.", categoria.Id);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al actualizar la categoría con ID {CategoriaId}.", categoria.Id);
                ModelState.AddModelError("", "Ocurrió un error al actualizar el registro.");
                return View(categoria);
            }

            TempData["SuccessMessage"] = "Categoría actualizada exitosamente.";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Consultando categoría con ID: {CategoriaId} para confirmación de eliminación.", id);
            Categoria categoria = null;

            try
            {
                using (var conexion = new SqlConnection(_connectionString))
                {
                    var sql = "SELECT Id, Nombre, Descripcion FROM Categorias WHERE Id = @Id";
                    using (var comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@Id", id);
                        conexion.Open();
                        using (var lector = comando.ExecuteReader())
                        {
                            if (lector.Read())
                            {
                                categoria = new Categoria
                                {
                                    Id = lector.GetInt32(0),
                                    Nombre = lector.GetString(1),
                                    Descripcion = lector.IsDBNull(2) ? null : lector.GetString(2)
                                };
                            }
                        }
                    }
                }

                if (categoria == null)
                {
                    _logger.LogWarning("Intento fallido: No se encontró la categoría con ID {CategoriaId} para eliminar.", id);
                    return NotFound();
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error al consultar la categoría con ID {CategoriaId} para eliminar.", id);
            }

            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            _logger.LogInformation("Iniciando eliminación física para la categoría con ID: {CategoriaId}", id);

            try
            {
                using (var conexion = new SqlConnection(_connectionString))
                {
                    var sql = "DELETE FROM Categorias WHERE Id = @Id";
                    using (var comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@Id", id);
                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }
                _logger.LogInformation("Categoría con ID {CategoriaId} eliminada exitosamente.", id);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error crítico al intentar eliminar la categoría con ID {CategoriaId}.", id);
            }

            TempData["SuccessMessage"] = "Categoría eliminada exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}