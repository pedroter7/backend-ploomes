namespace PloomesBackend.Data.Queries
{
    public class UsuarioQueryBuilder : IUsuarioQueryBuilder
    {
        public (object, string) BuildBuscarUsuarioPorEmail(string email)
        {
            const string query = "SELECT Id, Nome, Email FROM Usuario WHERE Email = @Email;";
            return (new
            {
                Email = email
            }, query);
        }

        public (object, string) BuildInserirUsuarioRetornandoId(string nome, string email, string senha)
        {
            const string query = "INSERT INTO Usuario (Nome, Email, Senha) VALUES (@Nome, @Email, @Senha); SELECT Id = scope_identity();";
            return (new
            {
                Nome = nome,
                Email = email,
                Senha = senha
            }, query);
        }

        public (object, string) BuildSelecionarPorEmailESenha(string email, string senha)
        {
            const string query = "SELECT Id, Nome, Email FROM Usuario WHERE Email = @Email AND Senha = @Senha";
            return (new
            {
                Senha = senha,
                Email = email,
            }, query);
        }
    }
}
