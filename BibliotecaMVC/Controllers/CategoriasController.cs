using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using BibliotecaMVC.Models;
using Microsoft.Data.SqlClient;

namespace BibliotecaMVC.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly string _connectionString;

        public CategoriasController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BibliotecaDB");
        }

        public IActionResult Index()
        {
            var categorias = new List<Categoria>();

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

            return View(categorias);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            if (categoria == null || string.IsNullOrEmpty(categoria.Nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre de la categoría es obligatorio.");
                return View(categoria);
            }

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

            TempData["SuccessMessage"] = "Categoría creada exitosamente.";
            return RedirectToAction("Index");
        }

        // GET: Categorias/Edit/5
        public IActionResult Edit(int id)
        {
            Categoria categoria = null;

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
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            if (categoria == null || string.IsNullOrEmpty(categoria.Nombre))
            {
                ModelState.AddModelError("Nombre", "El nombre de la categoría es obligatorio.");
                return View(categoria);
            }

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

            TempData["SuccessMessage"] = "Categoría actualizada exitosamente.";
            return RedirectToAction("Index");
        }

        // GET: Categorias/Delete/5
        public IActionResult Delete(int id)
        {
            Categoria categoria = null;

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
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
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

            TempData["SuccessMessage"] = "Categoría eliminada exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}