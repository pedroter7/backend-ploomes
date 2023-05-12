using PloomesBackend.Data.Models;

namespace PloomesBackend.Data.Queries
{
    public interface IClienteQueryBuilder
    {
        (object, string) BuildInserirClienteRetornandoId(long usuarioId, CriarClienteModel model);
        (object, string) BuildSelecionarClientesDoUsuario(long usuarioId);
    }
}
