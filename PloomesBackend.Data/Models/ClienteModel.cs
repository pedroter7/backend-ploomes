namespace PloomesBackend.Data.Models
{
    public class ClienteModel
    {
        public long Id { get; set; }
        public string Nome { get; set; } = null!;
        public DateTime DataCriacao { get; set; }
        public long UsuarioId { get; set; }
    }
}
