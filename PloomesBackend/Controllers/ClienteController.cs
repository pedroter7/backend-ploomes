using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PloomesBackend.Controllers.Extensions;
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
        private readonly IMapper _mapper;

        public ClienteController(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
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

            return Ok(_mapper.Map<IEnumerable<ClienteViewModel>>(result));
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
            var dataModel = _mapper.Map<CriarClienteModel>(model);

            try
            {
                var novoClienteId = await _clienteRepository.CriarCliente(usuarioId, dataModel);
                return this.CreatedJsonContentResult(new RecursoCriadoReturnViewModel { Id = novoClienteId });
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
