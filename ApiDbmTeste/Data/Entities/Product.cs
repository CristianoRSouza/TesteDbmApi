using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiDbmTeste.Data.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Nome { get; set; }

        public string? Descricao { get; set; }

        public decimal Preco { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}
