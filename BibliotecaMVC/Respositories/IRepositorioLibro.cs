using System.Collections.Generic;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Respositories
{
    public interface IRepositorioLibro
    {
        IEnumerable<Libro> ObtenerLibros();
        Libro ObtenerPorId(int id);
        void AgregarLibro(Libro libro);
        void ActualizarLibro(Libro libro);
        void EliminarLibro(Libro libro);
    }
}