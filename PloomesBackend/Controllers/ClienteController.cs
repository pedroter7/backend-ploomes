using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PloomesBackend.Data.Exceptions;
using PloomesBackend.Data.Models;
using PloomesBackend.Data.Repository;
using PloomesBackend.Security.Extensions;
using PloomesBackend.ViewModels;

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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<ClienteViewModel>), StatusCodes.Status200OK)]
        [Authorize("EmailSenha")]
        public async Task<IActionResult> ListarClientesParaUsuario()
        {
            var usuarioId = GetIdUsuarioAutenticado();
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
        [ProducesResponseType(typeof(RecursoCriadoReturnViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize("EmailSenha")]
        public async Task<IActionResult> CriarClienteParaUsuario([FromBody] CriarClienteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuarioId = GetIdUsuarioAutenticado();
            var dataModel = new CriarClienteModel
            {
                Nome = model.Nome
            };

            try
            {
                var novoClienteId = await _clienteRepository.CriarCliente(usuarioId, dataModel);
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new RecursoCriadoReturnViewModel { Id = novoClienteId }),
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status201Created
                };
            }
            catch (EntidadeJaExisteException)
            {
                return BadRequest(new
                {
                    detalhes = $"Já existe cliente com nome {dataModel.Nome} para o usuário"
                });
            }
        }

        private long GetIdUsuarioAutenticado()
        {
            var usuarioId = HttpContext.User.GetUsuarioId();
            if (usuarioId == null)
            {
                throw new Exception("Não foi possivel obter o ID do usuário autenticado");
            }

            return (long)usuarioId;
        }
    }
}
