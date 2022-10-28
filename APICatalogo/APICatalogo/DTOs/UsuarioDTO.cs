using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class UsuarioDTO
    {
        [Required(ErrorMessage = "Informe o seu email")]
        [EmailAddress(ErrorMessage = "Informe um email válido...")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Informe o sua senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Informe a Senha")]
        [StringLength(10, MinimumLength = 4)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme sua senha")]
        [Compare("Password", ErrorMessage = "A senha digitada não confere")]
        public string? ConfirmPassword { get; set; }
    }
}
