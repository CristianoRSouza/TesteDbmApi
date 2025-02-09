using ApiDbmTeste.Data.Dtos;
using ApiDbmTeste.Data.Entities;
using ApiDbmTeste.Interfaces.IServices;
using ApiDbmTeste.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiDbmTeste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IServiceProduct _UserService;
        public ProductController(IServiceProduct userService)
        {
            _UserService = userService;
        }
        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            return await _UserService.GetAllUser();
        }

        [HttpGet("{id}")]
        public async Task<ProductDto> Get(int id)
        {
            return await _UserService.GetUser(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProductDto product)
        {    
            try
            {
                await _UserService.AddUser(product);
                return Ok("Produto cadastrado com sucesso!");
            }
            catch (FluentValidation.ValidationException ex)
            {
                var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                return BadRequest(new { Message = "Erro de validação", Errors = errors });
            }
        }

        [HttpPut]
        public async Task Put([FromBody] ProductDto product)
        {
            await _UserService.UpdateUser(product);
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _UserService.DeleteUser(id);
        }
    }
}
