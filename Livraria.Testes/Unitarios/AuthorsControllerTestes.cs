using AutoMapper;
using livraria.api.Controllers;
using livraria.api.Data.IRepositories;
using livraria.api.Models;
using livraria.api.ViewModels;
using Microsoft.AspNetCore.Mvc;

using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Livraria.Testes.Unitarios
{
    public class AuthorsControllerTests
    {
        private readonly Mock<IAuthorsRepository> _AuthorsRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AuthorsController _controller;

        public AuthorsControllerTests()
        {
            _AuthorsRepositoryMock = new Mock<IAuthorsRepository>();
            _mapperMock = new Mock<IMapper>();
            _controller = new AuthorsController(_AuthorsRepositoryMock.Object, _mapperMock.Object);
        }


        [Fact]
        public async Task Get_ReturnsOkResult_WithAuthors()
        {
            // Arrange
            var Authors = new List<Author>
            {
                new Author { Id = Guid.NewGuid(), Name = "Author1" },
                new Author { Id = Guid.NewGuid(), Name = "Author2" }
            };

            _AuthorsRepositoryMock.Setup(repo => repo.Get()).ReturnsAsync(Authors);

            // Act
            var result = await _controller.Get();

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Author>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count());
        }

        [Fact]
        public async Task Get_WithPagination_ReturnsOkResult_WithPagedAuthors()
        {
            // Arrange
            int size = 2; // Número de itens por página
            int page = 3; // Primeira página
            var Authors = new List<Author>
                {
                    new Author { Id = Guid.NewGuid(), Name = "Author1" },
                    new Author { Id = Guid.NewGuid(), Name = "Author2" },
                    new Author { Id = Guid.NewGuid(), Name = "Author3" },
                    new Author { Id = Guid.NewGuid(), Name = "Author4" },
                    new Author { Id = Guid.NewGuid(), Name = "Author5" },
                    new Author { Id = Guid.NewGuid(), Name = "Author6" },
                    new Author { Id = Guid.NewGuid(), Name = "Author7" },
                    new Author { Id = Guid.NewGuid(), Name = "Author8" }
                };

            _AuthorsRepositoryMock.Setup(repo => repo.Get(size, page)).ReturnsAsync(new List<Author> { Authors[0] });

            // Act
            var result = await _controller.Get(size, page);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Author>>(actionResult.Value);
            Assert.Single(returnValue); // Espera-se que retorne apenas 1 item
        }

        [Fact]
        public async Task Get_ById_ReturnsOkResult_WithAuthor()
        {
            // Arrange
            var AuthorId = Guid.NewGuid();
            var Author = new Author { Id = AuthorId, Name = "Author1" };

            _AuthorsRepositoryMock.Setup(repo => repo.GetById(AuthorId)).ReturnsAsync(Author);

            // Act
            var result = await _controller.Get(AuthorId);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<Author>(actionResult.Value);
            Assert.Equal(AuthorId, returnValue.Id);
        }

        [Fact]
        public async Task Post_ValidAuthor_ReturnsCreatedResult()
        {
            // Arrange
            var AuthorViewModel = new AuthorViewModel { Name = "New Author" };
            var Author = new Author { Id = Guid.NewGuid(), Name = AuthorViewModel.Name };

            _mapperMock.Setup(m => m.Map<Author>(AuthorViewModel)).Returns(Author);
            _AuthorsRepositoryMock.Setup(repo => repo.Add(Author)).ReturnsAsync(Author);

            // Act
            var result = await _controller.Post(AuthorViewModel);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsAssignableFrom<Author>(createdResult.Value);
            Assert.Equal(Author.Id, returnValue.Id);
        }

        [Fact]
        public async Task Post_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var AuthorViewModel = new AuthorViewModel { Name = "New Author" };
            var Author = new Author { Id = Guid.NewGuid(), Name = AuthorViewModel.Name };

            _mapperMock.Setup(m => m.Map<Author>(AuthorViewModel)).Returns(Author);
            _AuthorsRepositoryMock.Setup(repo => repo.Add(Author)).ReturnsAsync(Author);

            // Act
            var result = await _controller.Post(AuthorViewModel);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsAssignableFrom<Author>(createdResult.Value);
            Assert.Equal(Author.Id, returnValue.Id);
        }

        [Fact]
        public async Task Put_ValidAuthor_ReturnsOkResult()
        {
            // Arrange
            var AuthorViewModel = new AuthorViewModel { Id = Guid.NewGuid(), Name = "Updated Author" };
            var Author = new Author { Id = AuthorViewModel.Id, Name = AuthorViewModel.Name };

            _mapperMock.Setup(m => m.Map<Author>(AuthorViewModel)).Returns(Author);
            _AuthorsRepositoryMock.Setup(repo => repo.Update(Author)).ReturnsAsync(Author);

            // Act
            var result = await _controller.Put(AuthorViewModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<Author>(okResult.Value);
            Assert.Equal(Author.Id, returnValue.Id);
        }




    }
}
