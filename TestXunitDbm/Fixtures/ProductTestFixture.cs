using ApiDbmTeste.Data.Entities;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestXunitDbm.Fixtures
{
    [CollectionDefinition(nameof(ProductCollection))]
    public class ProductCollection : ICollectionFixture<ProductTestFixture>
    { }
    public class ProductTestFixture : IDisposable
    {
        public Product GenerateValidProduct()
        {
            var productFaker = new Faker<Product>()
     .           RuleFor(p => p.Id, f => f.Random.Int(1, 1000)) 
                .RuleFor(p => p.Nome, f => f.Person.FirstName) 
                .RuleFor(p => p.Descricao, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.DataCadastro, f => f.Date.Past(1).Date) 
                .RuleFor(p => p.Preco, f => f.Finance.Amount(1000, 1000000, 2)).Generate();

            return productFaker;
        }

        public List<Product> GenerateListProduct()
        {
            var product = new Faker<Product>("pt_BR").RuleFor(p => p.Id, f => f.Random.Int(1, 100))
                .RuleFor(p => p.Nome, f => f.Name.FirstName()).RuleFor(p => p.Descricao, f => f.Lorem.Sentence())
                .RuleFor(p => p.DataCadastro, f => f.Date.Soon(110).Date).RuleFor(p => p.Preco, f => f.Finance.Amount(10, 100000));

            var produtos = product.Generate(20);

            return produtos;
        }


        public Product GenerateInvalidProduct()
        {
            var product = new Product
            {
                Id = new Faker().Random.Int(1,100),
                Nome = string.Empty,
                Descricao = string.Empty,
                DataCadastro = DateTime.Now.Date,
                Preco = 1000000
            };

            return product;
        }

        public void Dispose()
        {
        }
    }
}
