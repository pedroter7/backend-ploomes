using Dapper;
using Microsoft.Data.SqlClient;
using PloomesBackend.Data.Exceptions;
using PloomesBackend.Data.Models;
using PloomesBackend.Data.Queries;
using PloomesBackend.Data.Util;

namespace PloomesBackend.Data.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IUsuarioQueryBuilder _queryBuilder;
        private readonly string _connectionString;

        private SqlConnection Connection => new(_connectionString);

        public UsuarioRepository(IUsuarioQueryBuilder queryBuilder, IConnectionStringGetter connectionStringGetter)
        {
            _queryBuilder = queryBuilder;
            _connectionString = connectionStringGetter.GetConnectionString();
        }

        public async Task<bool> ChecarDadosAutenticacaoUsuario(string email, string senha)
        {
            await using var conn = Connection;
            var (pars, query) = _queryBuilder.BuildSelecionarPorEmailESenha(email, senha);
            var result = await conn.QueryAsync<UsuarioModel>(query, pars);
            return result.Any();
        }

        public async Task<long> InserirUsuario(string nome, string email, string senha)
        {
            await using var conn = Connection;
            var (pars, query) = _queryBuilder.BuildInserirUsuarioRetornandoId(nome, email, senha);
            try
            {
                return (await conn.QueryAsync<long>(query, pars)).First();
            }
            catch (SqlException e)
            {
                if (e.Number == (int)SqlExceptionTypeEnum.UNIQUE_KEY_CONSTRAINT_VIOLATION)
                {
                    throw new EntidadeJaExisteException();
                }

                throw;
            }
        }

        public async Task<UsuarioModel> BuscarUsuario(string email)
        {
            await using var conn = Connection;
            var (pars, query) = _queryBuilder.BuildBuscarUsuarioPorEmail(email);
            return (await conn.QueryAsync<UsuarioModel>(query, pars)).First();
        }
    }
}
