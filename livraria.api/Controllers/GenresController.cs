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
    public class GenresController : ControllerBase
    {
        private readonly IGenresRepository _GenresRepository;
        private readonly IMapper _mapper;
        public GenresController(IGenresRepository GenresRepository, IMapper mapper)
        {
            _GenresRepository = GenresRepository;
            _mapper = mapper;
        }


        #region Genres
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var items = await _GenresRepository.Get();
                return Ok(items);
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
                var item = await _GenresRepository.Get(size, page);
                return Ok(item);
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
                var item = await _GenresRepository.GetById(Id);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenreViewModel itemVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var mapeado = _mapper.Map<Genre>(itemVM);
                var item = await _GenresRepository.Add(mapeado);
                return CreatedAtAction(nameof(Get), new { Id = item.Id }, item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPut]
        public async Task<ActionResult> Put([FromBody] GenreViewModel itemVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var mapeado = _mapper.Map<Genre>(itemVM);
                var item = await _GenresRepository.Update(mapeado);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpDelete]
        [Route("{Id}")]
        public async Task<ActionResult> DeleteGenre(Guid Id)
        {
            try
            {
                // Tenta obter a categoria pelo ID
                var item = await _GenresRepository.GetById(Id);
                if (item == null)
                {
                    return NotFound(); // Retorna NotFound se a categoria não existir
                }

                await _GenresRepository.Delete(Id); // Executa a exclusão
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
