using System.ComponentModel.DataAnnotations;

namespace PloomesBackend.ViewModels
{
    public class CriarClienteViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(250)]
        public string Nome { get; set; } = null!;
    }
}
