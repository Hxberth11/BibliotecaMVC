using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using BibliotecaMVC.Controllers;
using BibliotecaMVC.Data;
using BibliotecaMVC.Models;
using System.Collections.Generic;

namespace BibliotecaMVC.Tests
{
    public class LibrosPruebas
    {
        private BibliotecaContext ObtenerContextoEnMemoria()
        {
            var opciones = new DbContextOptionsBuilder<BibliotecaContext>()
                .UseInMemoryDatabase(databaseName: $"BibliotecaTestDb_{Guid.NewGuid()}")
                .Options;

            var contexto = new BibliotecaContext(opciones);
            return contexto;
        }

        [Fact]
        public async Task Index_ConLibros_DebeRetornarVistaYModeloNoVacio()
        {
            // Arrange
            using var context = ObtenerContextoEnMemoria();
            context.Libros.Add(new Libro { Id = 1, Titulo = "Clean Code", Autor = "Robert C. Martin" });
            context.Libros.Add(new Libro { Id = 2, Titulo = "Refactoring", Autor = "Martin Fowler" });
            await context.SaveChangesAsync();

            var mockLogger = new Mock<ILogger<LibrosController>>();
            var controller = new LibrosController(context, mockLogger.Object);

            // Act
            var resultado = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(resultado);
            var modelo = Assert.IsAssignableFrom<List<Libro>>(viewResult.Model);

            Assert.NotNull(viewResult);
            Assert.NotNull(modelo);
            Assert.False(modelo.Count == 0);
            Assert.True(modelo.Count > 0);
            Assert.Equal(2, modelo.Count);
        }

        [Fact]
        public async Task Index_SinLibros_DebeRetornarListaVacia()
        {
            // Arrange
            using var context = ObtenerContextoEnMemoria();
            var mockLogger = new Mock<ILogger<LibrosController>>();
            var controller = new LibrosController(context, mockLogger.Object);

            // Act
            var resultado = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(resultado);
            var modelo = Assert.IsAssignableFrom<List<Libro>>(viewResult.Model);

            Assert.NotNull(modelo);
            Assert.Empty(modelo);
        }

        [Fact]
        public async Task Details_ConLibroExistente_DebeRetornarVistaConLibro()
        {
            // Arrange
            using var context = ObtenerContextoEnMemoria();
            context.Libros.Add(new Libro { Id = 10, Titulo = "Pragmatic Programmer", Autor = "Andy Hunt" });
            await context.SaveChangesAsync();

            var mockLogger = new Mock<ILogger<LibrosController>>();
            var controller = new LibrosController(context, mockLogger.Object);

            // Act
            var resultado = await controller.Details(10);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(resultado);
            var libro = Assert.IsAssignableFrom<Libro>(viewResult.Model);
            Assert.NotNull(libro);
            Assert.Equal(10, libro.Id);
            Assert.Equal("Pragmatic Programmer", libro.Titulo);
        }

        
        [Theory]
        [InlineData(null)]     // ID Nulo
        [InlineData(999)]    // ID que no existe en la base de datos
        [InlineData(-5)]     // ID inválido/negativo
        public async Task Details_ConIdInvalidoONoExistente_DebeRetornarNotFound(int? id)
        {
            // Arrange
            using var context = ObtenerContextoEnMemoria();
            var mockLogger = new Mock<ILogger<LibrosController>>();
            var controller = new LibrosController(context, mockLogger.Object);

            // Act
            var resultado = await controller.Details(id);

            // Assert
            Assert.NotNull(resultado);
            Assert.IsType<NotFoundResult>(resultado);
        }
    }
}