using System.ComponentModel.DataAnnotations;

namespace PloomesBackend.ViewModels
{
    public class CriarUsuarioViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(250)]
        public string Nome { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        [MaxLength(150)]
        public string Email { get; set; } = null!;

        [Required(AllowEmptyStrings = false)]
        [MaxLength(30)]
        public string Senha { get; set; } = null!;
    }
}
