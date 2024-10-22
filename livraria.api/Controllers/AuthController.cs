using livraria.api.Data.IRepositories;
using livraria.api.DTO;
using livraria.api.Enums;
using livraria.api.Models;
using livraria.api.Services;
using livraria.api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace livraria.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        protected readonly IUsersRepository _usersRepository;
        protected readonly IJWTService _tokensService;
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public AuthController(UserManager<User> userManager, IConfiguration configuration,
            IUsersRepository usersRepository, IJWTService jWTService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _usersRepository = usersRepository;
            _tokensService = jWTService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> LoginAsync(LoginViewModel login)
        {
            var user = await _userManager.FindByEmailAsync(login.EmailPhone);

            if (user == null)
            {
                var lista = _usersRepository.Get().Result;
                user = lista.Where(q => q.PhoneNumber == login.EmailPhone).FirstOrDefault();
            }

            if (user is not null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                if (!user.Active) return BadRequest("Utilizador Bloqueado, Contacte o administrador");

                return Ok(new ResponseDTO
                {
                    Data = _tokensService.GetToken(user)
                });
            }
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ActionResult> Registrar(SignupViewModel registerUser)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Retorna os erros de validação
            }

            var user = new User
            {
                FullName = registerUser.FullName,
                PhoneNumber = registerUser.PhoneNumber,
                UserName = registerUser.Email,
                Email = registerUser.Email,
                UserType = EUserType.Basic,
                EmailConfirmed = true,
                Active = true,
            };

            if (string.IsNullOrEmpty(registerUser.Email) && string.IsNullOrEmpty(registerUser.PhoneNumber)) return BadRequest("Email ou Telefone são obrigatórios");


            if (!string.IsNullOrEmpty(registerUser.PhoneNumber))
            {
                if (!validaPhone(registerUser.PhoneNumber)) return BadRequest("Numero de Telefone Inválido");
                if (_usersRepository.ExisteByPhoneNumber(registerUser.PhoneNumber).Result) return BadRequest("O Numero de telefone informado já foi cadastrado");
            }


            if (!string.IsNullOrEmpty(registerUser.Email))
            {
                if (!EmailRegex.IsMatch(registerUser.Email)) return BadRequest("O Email informado é inválido");
                if (_usersRepository.ExisteByMail(registerUser.Email).Result) return BadRequest("O Email informado já foi cadastrado");
            }

            if (string.IsNullOrEmpty(registerUser.Email)) user.UserName = registerUser.PhoneNumber;


            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {

                var login = new LoginViewModel { Password = registerUser.Password };

                if (!string.IsNullOrEmpty(registerUser.Email)) login.EmailPhone = registerUser.Email;
                else login.EmailPhone = registerUser.PhoneNumber;

                return await LoginAsync(login);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        bool validaPhone(string phoneNumber)
        {
            if (phoneNumber.Length != 9) return false;
            if (!phoneNumber.StartsWith("9")) return false;
            return true;
        }

    }
}