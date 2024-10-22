using AutoMapper;
using livraria.api.Data.IRepositories;
using livraria.api.Models;
using livraria.api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace livraria.api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _UsersRepository;
        private readonly IMapper _mapper;
        public UsersController(IUsersRepository UsersRepository, IMapper mapper)
        {
            _UsersRepository = UsersRepository;
            _mapper = mapper;
        }


        #region Users
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var items = await _UsersRepository.Get();
                var mapeado = _mapper.Map<IEnumerable<UserViewModel>>(items);
                return Ok(mapeado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        [HttpGet]
        [Route("{size}/{page}")]
        public async Task<IActionResult> Get(int size, int page)
        {
            try
            {
                var items = await _UsersRepository.Get(size, page);
                var mapeado = _mapper.Map<IEnumerable<UserViewModel>>(items);
                return Ok(mapeado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> Get(Guid Id)
        {
            try
            {
                var item = await _UsersRepository.GetById(Id);
                var mapeado = _mapper.Map<UserViewModel>(item);
                return Ok(mapeado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


       

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] UserViewModel itemVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var mapeado = _mapper.Map<User>(itemVM);
                var item = await _UsersRepository.Update(mapeado);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpDelete]
        [Route("{Id}")]
        public async Task<ActionResult> DeleteUser(Guid Id)
        {
            try
            {
                // Tenta obter a categoria pelo ID
                var item = await _UsersRepository.GetById(Id);
                if (item == null)
                {
                    return NotFound(); // Retorna NotFound se a categoria não existir
                }

                await _UsersRepository.Delete(Id); // Executa a exclusão
                return Ok(); // Retorna Ok() após a exclusão
            }
            catch (Exception ex)
            {
                return BadRequest(ex); // Retorna BadRequest em caso de erro
            }
        }
        #endregion
    }
}
