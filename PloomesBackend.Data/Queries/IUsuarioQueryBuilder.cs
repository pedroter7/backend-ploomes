namespace PloomesBackend.Data.Queries
{
    public interface IUsuarioQueryBuilder
    {
        (object, string) BuildInserirUsuarioRetornandoId(string nome, string email, string senha);
        (object, string) BuildSelecionarPorEmailESenha(string email, string senha);
        (object, string) BuildBuscarUsuarioPorEmail(string email);
    }
}
