using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations;
using System;

namespace ApiDbmTeste.Data.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
   
        public string Nome { get; set; }

        public string? Descricao { get; set; }

        public decimal Preco { get; set; }
    }
}
