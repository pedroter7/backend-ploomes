using Dapper;
using Microsoft.Data.SqlClient;
using PloomesBackend.Data.Models;
using PloomesBackend.Data.Queries;
using PloomesBackend.Data.Util;

namespace PloomesBackend.Data.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IClienteQueryBuilder _clienteQueryBuilder;
        private readonly string _connectionString;

        private SqlConnection Connection => new(_connectionString);

        public ClienteRepository(IClienteQueryBuilder clienteQueryBuilder, IConnectionStringGetter connectionStringGetter)
        {
            _clienteQueryBuilder = clienteQueryBuilder;
            _connectionString = connectionStringGetter.GetConnectionString();
        }

        public async Task<long> CriarCliente(long usuarioId, CriarClienteModel model)
        {
            var (pars, query) = _clienteQueryBuilder.BuildInserirClienteRetornandoId(usuarioId, model);
            await using var conn = Connection;
            return (await conn.QueryAsync<long>(query, pars)).First();
        }

        public async Task<IEnumerable<ClienteModel>> ListarClientesParaUsuario(long id)
        {
            var (pars, query) = _clienteQueryBuilder.BuildSelecionarClientesDoUsuario(id);
            await using var conn = Connection;
            return await conn.QueryAsync<ClienteModel>(query, pars);
        }
    }
}
