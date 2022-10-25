using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.DTOs
{
    public class ProdutoDTO
    {
        public int ProdutoId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "O nome deve conter entre 5 e 50 caracteres - *Campo Obrigatório*", MinimumLength = 5)]
        public string? Nome { get; set; }
        [Required]
        [StringLength(300, ErrorMessage = "A descrição deve conter entre {2} até {1} caracteres", MinimumLength = 5)]
        public string? Descricao { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(1, 10000, ErrorMessage = "O preço deve estar entre {1} e {2}")]
        public decimal Preco { get; set; }
        [Required(ErrorMessage = "Digite a url da imagem - *Campo Obrigatório*")]
        [MaxLength(80)]
        public string? ImagemUrl { get; set; }
        [Range(1, 30, ErrorMessage = "O id da categoria deve estar entre {1} e {2}")]
        public int CategoriaId { get; set; }

    }
}
