using System.Collections.Generic;
using System.Linq;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Respositories
{
    public class RepositorioEnMemoria : IRepositorioLibro
    {
        private static readonly List<Libro> _libros = new List<Libro>
        {
            new Libro {
                Id = 1,
                Titulo = "Cien Años de Soledad",
                Autor = "Gabriel García Márquez",
                Genero = "Realismo Mágico",
                AnioPublicacion = 1967,
                ImagenUrl = "cien_anios.webp",
                Precio = 22.50m,
                Disponible = true
            },
            new Libro {
                Id = 2,
                Titulo = "La Casa de los Espíritus",
                Autor = "Isabel Allende",
                Genero = "Novela",
                AnioPublicacion = 1982,
                ImagenUrl = "default.jpg",
                Precio = 18.00m,
                Disponible = true
            },
            new Libro {
                Id = 3,
                Titulo = "Ficciones",
                Autor = "Jorge Luis Borges",
                Genero = "Ficción",
                AnioPublicacion = 1944,
                ImagenUrl = "default.jpg",
                Precio = 15.75m,
                Disponible = false
            },
            new Libro {
                Id = 4,
                Titulo = "Don Quijote de la Mancha",
                Autor = "Miguel de Cervantes",
                Genero = "Novela Clásica",
                AnioPublicacion = 1605,
                ImagenUrl = "default.jpg",
                Precio = 28.99m,
                Disponible = true
            },
            new Libro {
                Id = 5,
                Titulo = "Rayuela",
                Autor = "Julio Cortázar",
                Genero = "Novela",
                AnioPublicacion = 1963,
                ImagenUrl = "default.jpg",
                Precio = 20.50m,
                Disponible = false
            }
        };

        public IEnumerable<Libro> ObtenerLibros()
        {
            return _libros;
        }

        public Libro? ObtenerPorId(int id)
        {
            return _libros.FirstOrDefault(l => l.Id == id);
        }

        public void AgregarLibro(Libro libro)
        {
            libro.Id = _libros.Any() ? _libros.Max(l => l.Id) + 1 : 1;
            _libros.Add(libro);
        }

        public void ActualizarLibro(Libro libroModificado)
        {
            var libroExistente = ObtenerPorId(libroModificado.Id);
            if (libroExistente != null)
            {
                libroExistente.Titulo = libroModificado.Titulo;
                libroExistente.Autor = libroModificado.Autor;
                libroExistente.Genero = libroModificado.Genero;
                libroExistente.AnioPublicacion = libroModificado.AnioPublicacion;
                libroExistente.ImagenUrl = libroModificado.ImagenUrl;

                // Actualizamos las nuevas propiedades
                libroExistente.Precio = libroModificado.Precio;
                libroExistente.Disponible = libroModificado.Disponible;
            }
        }

        public void EliminarLibro(Libro libro)
        {
            var libroExistente = ObtenerPorId(libro.Id);
            if (libroExistente != null)
            {
                _libros.Remove(libroExistente);
            }
        }
    }
}
