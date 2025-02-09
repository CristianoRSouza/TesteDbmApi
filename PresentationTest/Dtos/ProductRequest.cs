namespace PresentationTest.Dtos
{
    public class ProductRequest
    {
        public int? Id { get; set; } = 0;
   
        public string Nome { get; set; }

        public string? Descricao { get; set; }

        public decimal Preco { get; set; }

    }
}
