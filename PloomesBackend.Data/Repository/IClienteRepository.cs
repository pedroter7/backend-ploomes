using PloomesBackend.Data.Models;

namespace PloomesBackend.Data.Repository
{
    public interface IClienteRepository
    {
        Task<IEnumerable<ClienteModel>> ListarClientesParaUsuario(long id);
        Task<long> CriarCliente(long usuarioId, CriarClienteModel model);
    }
}
