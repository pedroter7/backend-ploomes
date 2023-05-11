using Microsoft.AspNetCore.Mvc;
using PloomesBackend.Data.Repository;
using PloomesBackend.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace PloomesBackend.Controllers
{
    [ApiController]
    [Route("api/v1/clientes")]
    public class ClienteController : Controller
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<ClienteViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListarClientesParaUsuario([FromQuery][Range(1L, long.MaxValue)] long usuarioId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _clienteRepository.ListarClientesParaUsuario(usuarioId);
            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result.Select(c => new ClienteViewModel
            {
                CriadoData = c.DataCriacao,
                Id = c.Id,
                Nome = c.Nome
            }));
        }

        [HttpPost]
        public Task<IActionResult> CriarClienteParaUsuario([FromQuery][Range(1L, long.MaxValue)] long usuarioId, 
            [FromBody] CriarClienteViewModel model)
        {

        }
    }
}
