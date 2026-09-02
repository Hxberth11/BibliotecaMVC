using System;
using System.Collections.Generic;
using System.Linq;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Services
{
    // Segunda implementación para cumplir el Reto (Actividad 5)
    public class AutorMockService : IAutorService
    {
        private static readonly List<Autor> _AutoresPrueba = new List<Autor>
        {
            new Autor { Id = 101, Nombre = "Miguel", Apellido = "de Cervantes", Nacionalidad = "Española", FechaNacimiento = new DateTime(1547, 9, 29), Activo = true },
            new Autor { Id = 102, Nombre = "Octavio", Apellido = "Paz", Nacionalidad = "Mexicana", FechaNacimiento = new DateTime(1914, 3, 31), Activo = true }
        };

        public IEnumerable<Autor> ObtenerTodos() => _AutoresPrueba;

        public Autor? ObtenerPorId(int id) => _AutoresPrueba.FirstOrDefault(a => a.Id == id);

        public void Actualizar(Autor autorActualizado)
        {
            var autor = _AutoresPrueba.FirstOrDefault(a => a.Id == autorActualizado.Id);
            if (autor != null)
            {
                autor.Nombre = autorActualizado.Nombre;
                autor.Apellido = autorActualizado.Apellido;
                autor.Nacionalidad = autorActualizado.Nacionalidad;
                autor.FechaNacimiento = autorActualizado.FechaNacimiento;
                autor.Activo = autorActualizado.Activo;
            }
        }

        public void Eliminar(int id)
        {
            var autor = _AutoresPrueba.FirstOrDefault(a => a.Id == id);
            if (autor != null) _AutoresPrueba.Remove(autor);
        }
    }
}