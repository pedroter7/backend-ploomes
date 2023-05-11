using PloomesBackend.Data.Models;

namespace PloomesBackend.Data.Repository
{
    public interface IUsuarioRepository
    {
        public Task<bool> ChecarDadosAutenticacaoUsuario(string email, string senha);
        public Task<long> InserirUsuario(string nome, string email, string senha);
        public Task<UsuarioModel> BuscarUsuario(string email);
    }
}
