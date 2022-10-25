using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class CategoriaDTO
    {
        public int CategoriaId { get; set; }
        [Required]
        [StringLength(80, ErrorMessage = "O nome deve conter entre 5 e 80 caracteres", MinimumLength = 5)]
        public string? Nome { get; set; }
        [Required(ErrorMessage = "Digite a url da imagem - *Campo Obrigatório*")]
        [MaxLength(80)]
        public string? ImagemUrl { get; set; }
        public ICollection<ProdutoDTO>? Produtos { get; set; }
    }
}
