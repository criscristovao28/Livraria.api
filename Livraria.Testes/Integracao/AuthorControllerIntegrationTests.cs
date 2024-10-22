using AutoMapper;
using livraria.api.Controllers;
using livraria.api.Data;
using livraria.api.Data.Repositories;
using livraria.api.Models;
using livraria.api.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Livraria.Testes.Integracao
{
    public class AuthorsControllerIntegrationTests
    {
        private readonly LibraryContext _context;
        private readonly AuthorsController _controller;

        public AuthorsControllerIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new LibraryContext(options);
            var repository = new AuthorsRepository(_context);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<AuthorViewModel, Author>()).CreateMapper();
            _controller = new AuthorsController(repository, mapper);
        }

        [Fact]
        public async Task Post_ValidAuthor_ReturnsCreatedResult()
        {
            // Arrange
            var AuthorViewModel = new AuthorViewModel { Name = "New Author" };

            // Act
            var result = await _controller.Post(AuthorViewModel);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsAssignableFrom<Author>(createdResult.Value);
            Assert.Equal("New Author", returnValue.Name);

            // Verifica se a categoria foi realmente adicionada ao banco de dados
            var AuthorInDb = await _context.Authors.FindAsync(returnValue.Id);
            Assert.NotNull(AuthorInDb);
            Assert.Equal("New Author", AuthorInDb.Name);
        }

        [Fact]
        public async Task Get_ReturnsAllAuthors()
        {
            // Arrange
            var Author1 = new Author { Name = "Author 1" };
            var Author2 = new Author { Name = "Author 2" };
            await _context.Authors.AddRangeAsync(Author1, Author2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var Authors = Assert.IsAssignableFrom<IEnumerable<Author>>(okResult.Value);
            Assert.Equal(2, Authors.Count());
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var Author = new Author { Id = Guid.NewGuid(), Name = "Existing Author" };
            await _context.Authors.AddAsync(Author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.Get((Guid)Author.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<Author>(okResult.Value);
            Assert.Equal(Author.Id, returnValue.Id);
        }



        [Fact]
        public async Task Delete_ExistingId_ReturnsOkResult()
        {
            // Arrange
            var Author = new Author { Id = Guid.NewGuid(), Name = "Author to Delete" };
            await _context.Authors.AddAsync(Author);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteAuthor((Guid)Author.Id);

            // Assert
            Assert.IsType<OkResult>(result); // Verifica se o resultado é do tipo OkResult

            // Verifica se a categoria foi realmente removida do banco de dados
            var AuthorInDb = await _context.Authors.FindAsync(Author.Id);
            Assert.Null(AuthorInDb);
        }


    }
}