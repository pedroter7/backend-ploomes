namespace PloomesBackend.Data.Models
{
    public class UsuarioModel
    {
        public long Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
