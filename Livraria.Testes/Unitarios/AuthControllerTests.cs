using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using livraria.api.Data.IRepositories;
using livraria.api.Services;
using livraria.api.Controllers;
using livraria.api.Models;
using livraria.api.ViewModels;
using livraria.api.DTO;
using livraria.api.Enums;

namespace Livraria.Testes.Unitarios
{
    public class AuthControllerTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IUsersRepository> _usersRepositoryMock;
        private readonly Mock<IJWTService> _tokensServiceMock;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            var store = new Mock<IUserStore<User>>();
            _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _configurationMock = new Mock<IConfiguration>();
            _usersRepositoryMock = new Mock<IUsersRepository>();
            _tokensServiceMock = new Mock<IJWTService>();

            _authController = new AuthController(
                _userManagerMock.Object,
                _configurationMock.Object,
                _usersRepositoryMock.Object,
                _tokensServiceMock.Object);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var loginModel = new LoginViewModel { EmailPhone = "test@example.com", Password = "Password123" };
            var user = new User { Email = "test@example.com", Active = true, Id = Guid.NewGuid(), UserType = EUserType.Basic };

            _userManagerMock.Setup(um => um.FindByEmailAsync(loginModel.EmailPhone)).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.CheckPasswordAsync(user, loginModel.Password)).ReturnsAsync(true);

            var tokenResponse = new TokenResponseDTO
            {
                Token = "fake-token",
                ValidTo = DateTime.UtcNow.AddHours(2),
                UserId = user.Id,
                UserType = user.UserType
            };

            _tokensServiceMock.Setup(ts => ts.GetToken(user)).Returns(tokenResponse);

            // Act
            var result = await _authController.LoginAsync(loginModel);

            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDTO>(actionResult.Value);
            Assert.Equal(tokenResponse, response.Data);
        }

        [Fact]
        public async Task Registrar_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var registerModel = new SignupViewModel
            {
                FullName = "",
                Email = "invalidemail",
                PhoneNumber = "invalidphone",
                Password = "123",
                ConfirmPassword = "1234"
            };

            _authController.ModelState.AddModelError("FullName", "O nome completo é obrigatório.");

            // Act
            var result = await _authController.Registrar(registerModel);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(actionResult.Value);
        }

        // Adicione mais testes para os outros cenários...
    }
}