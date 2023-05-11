using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

            var id = await _usuarioRepository.InserirUsuario(model.Nome, model.Email, model.Senha);
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(new RecursoCriadoReturnViewModel { Id = id}),
                ContentType = "application/json",
                StatusCode = StatusCodes.Status201Created
            };
        }
    }
}
