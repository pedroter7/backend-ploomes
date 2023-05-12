using Microsoft.AspNetCore.Mvc;
using PloomesBackend.Controllers.Extensions;
using PloomesBackend.Data.Exceptions;
using PloomesBackend.Data.Repository;
using PloomesBackend.ViewModels;

namespace PloomesBackend.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost]
        [ProducesResponseType(typeof(RecursoCriadoReturnViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var id = await _usuarioRepository.InserirUsuario(model.Nome, model.Email, model.Senha);
                return this.CreatedJsonContentResult(new RecursoCriadoReturnViewModel { Id = id });
            }
            catch (EntidadeJaExisteException)
            {
                return BadRequest(new
                {
                    detalhes = $"Já existe usuário cadastrado com email {model.Email}"
                });
            }
        }
    }
}
