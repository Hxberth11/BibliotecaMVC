using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Services
{
    public class AutorService : IAutorService
    {
        private static readonly List<Autor> _Autores = new List<Autor>
        {
            new Autor { Id = 1, Nombre = "Gabriel", Apellido = "García Márquez", Nacionalidad = "Colombiana", FechaNacimiento = new DateTime(1927, 3, 6), Activo = true },
            new Autor { Id = 2, Nombre = "Isabel", Apellido = "Allende", Nacionalidad = "Chilena", FechaNacimiento = new DateTime(1942, 8, 2), Activo = true },
            new Autor { Id = 3, Nombre = "Jorge Luis", Apellido = "Borges", Nacionalidad = "Argentina", FechaNacimiento = new DateTime(1899, 8, 24), Activo = false },
            new Autor { Id = 4, Nombre = "Mario", Apellido = "Vargas Llosa", Nacionalidad = "Peruana", FechaNacimiento = new DateTime(1936, 3, 28), Activo = true },
            new Autor { Id = 5, Nombre = "Julio", Apellido = "Cortázar", Nacionalidad = "Argentina", FechaNacimiento = new DateTime(1914, 8, 26), Activo = false }
        };

        public IEnumerable<Autor> ObtenerTodos()
        {
            return _Autores;
        }

        public Autor? ObtenerPorId(int id)
        {
            return _Autores.FirstOrDefault(a => a.Id == id);
        }

        public void Actualizar(Autor autorActualizado)
        {
            var autorExistente = _Autores.FirstOrDefault(a => a.Id == autorActualizado.Id);
            if (autorExistente != null)
            {
                autorExistente.Nombre = autorActualizado.Nombre;
                autorExistente.Apellido = autorActualizado.Apellido;
                autorExistente.Nacionalidad = autorActualizado.Nacionalidad;
                autorExistente.FechaNacimiento = autorActualizado.FechaNacimiento;
                autorExistente.Activo = autorActualizado.Activo;
            }
        }

        public void Eliminar(int id)
        {
            var autor = _Autores.FirstOrDefault(a => a.Id == id);
            if (autor != null)
            {
                _Autores.Remove(autor);
            }
        }
    }
}