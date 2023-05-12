using PloomesBackend.Data.Models;

namespace PloomesBackend.Data.Queries
{
    public class ClienteQueryBuilder : IClienteQueryBuilder
    {
        public (object, string) BuildInserirClienteRetornandoId(long usuarioId, CriarClienteModel model)
        {
            const string query = "INSERT INTO Cliente (Nome, DataCriacao, UsuarioId) VALUES (@Nome, @DataCriacao, @UsuarioId); SELECT Id = SCOPE_IDENTITY();";
            return (new
            {
                Nome = model.Nome,
                DataCriacao = DateTime.Now,
                UsuarioId = usuarioId
            }, query);
        }

        public (object, string) BuildSelecionarClientesDoUsuario(long usuarioId)
        {
            const string query = "SELECT c.Id, c.Nome, c.DataCriacao, c.UsuarioId FROM Cliente c WHERE c.UsuarioId = @UsuarioId";
            return (new
            {
                UsuarioId = usuarioId
            }, query);
        }
    }
}
