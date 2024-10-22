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
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository _BooksRepository;
        private readonly IMapper _mapper;
        public BooksController(IBooksRepository BooksRepository, IMapper mapper)
        {
            _BooksRepository = BooksRepository;
            _mapper = mapper;
        }


        #region Books
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var items = await _BooksRepository.Get();
                var mapeado = _mapper.Map< IEnumerable<BookFullViewModel>>(items);
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
                var item = await _BooksRepository.Get(size, page);
                var mapeado = _mapper.Map< IEnumerable<BookFullViewModel>>(item);
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
                var item = await _BooksRepository.GetById(Id);
                var mapeado = _mapper.Map<BookFullViewModel>(item);
                return Ok(mapeado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] BookViewModel itemVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (Guid.Empty == itemVM.GenreId) throw new Exception("Genero inválido");
                var item = await _BooksRepository.Add(itemVM);
                return CreatedAtAction(nameof(Get), new { Id = item.Id }, item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        //[HttpPut]
        //public async Task<ActionResult> Put([FromBody] BookViewModel itemVM)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        var mapeado = _mapper.Map<Book>(itemVM);
        //        var item = await _BooksRepository.Update(mapeado);
        //        return Ok(item);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }

        //}

        [HttpDelete]
        [Route("{Id}")]
        public async Task<ActionResult> DeleteBook(Guid Id)
        {
            try
            {
                // Tenta obter a categoria pelo ID
                var item = await _BooksRepository.GetById(Id);
                if (item == null)
                {
                    return NotFound(); // Retorna NotFound se a categoria não existir
                }

                await _BooksRepository.Delete(Id); // Executa a exclusão
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
