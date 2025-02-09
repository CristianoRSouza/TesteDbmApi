using ApiDbmTeste.Data.Dtos;
using ApiDbmTeste.Data.Entities;
using ApiDbmTeste.Interfaces.IRepositories;
using ApiDbmTeste.Services;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TestXunitDbm.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidator<ProductDto>> _validatorMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _validatorMock = new Mock<IValidator<ProductDto>>();
            _productService = new ProductService(_productRepositoryMock.Object, _mapperMock.Object, _validatorMock.Object);
        }

        [Fact(DisplayName = "Adicionar Produto com sucesso")]
        [Trait("Categoria", "Teste de Serviços Prdutos")]
        public async Task AddProduct_DeveAdicionarProduto_QuandoValido()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Nome = "Produto Teste", Preco = 100 };
            var product = new Product { Id = 1, Nome = "Produto Teste", Preco = 100 };

            _validatorMock.Setup(v => v.ValidateAsync(productDto, default))
                .ReturnsAsync(new ValidationResult());

            _mapperMock.Setup(m => m.Map<Product>(productDto)).Returns(product);

            // Act
            await _productService.AddProduct(productDto);

            // Assert
            _productRepositoryMock.Verify(r => r.Add(product), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Produto inválido deve lançar exceção")]
        [Trait("Categoria", "Teste de Serviços Prdutos")]
        public async Task AddProduct_DeveLancarExcecao_QuandoInvalido()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Nome = "", Preco = 100 };

            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Nome", "Nome é obrigatório.")
            };

            _validatorMock.Setup(v => v.ValidateAsync(productDto, default))
                .ReturnsAsync(new ValidationResult(validationFailures));

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _productService.AddProduct(productDto));
        }

        [Fact(DisplayName = "Excluir Produto com sucesso")]
        [Trait("Categoria", "Teste de Serviços Prdutos")]
        public async Task DeleteProduct_DeveRemoverProduto()
        {
            // Arrange
            int productId = 1;

            _productRepositoryMock.Setup(r => r.Delete(productId)).Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteProduct(productId);

            // Assert
            _productRepositoryMock.Verify(r => r.Delete(productId), Times.Once);
        }

        [Fact(DisplayName = "Obter todos os Produtos")]
        [Trait("Categoria", "Teste de Serviços Prdutos")]
        public async Task GetAllProduct_DeveRetornarListaDeProdutos()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = 1, Nome = "Produto Teste", Preco = 100 } };
            var productDtos = new List<ProductDto> { new ProductDto { Id = 1, Nome = "Produto Teste", Preco = 100 } };

            _productRepositoryMock.Setup(r => r.GetAll()).ReturnsAsync(products);
            _mapperMock.Setup(m => m.Map<IEnumerable<ProductDto>>(products)).Returns(productDtos);

            // Act
            var result = await _productService.GetAllProduct();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact(DisplayName = "Obter Produto por ID")]
        [Trait("Categoria", "Teste de Serviços Prdutos")]
        public async Task GetProduct_DeveRetornarProduto_QuandoExistente()
        {
            // Arrange
            var product = new Product { Id = 1, Nome = "Produto Teste", Preco = 100 };
            var productDto = new ProductDto { Id = 1, Nome = "Produto Teste", Preco = 100 };

            _productRepositoryMock.Setup(r => r.Get(1)).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

            // Act
            var result = await _productService.GetProduct(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productDto.Id, result.Id);
        }

        [Fact(DisplayName = "Atualizar Produto existente")]
        [Trait("Categoria", "Teste de Serviços Prdutos")]
        public async Task UpdateProduct_DeveAtualizarProduto_QuandoExistente()
        {
            // Arrange
            var productDto = new ProductDto { Id = 1, Nome = "Produto Atualizado", Preco = 150 };
            var existingProduct = new Product { Id = 1, Nome = "Produto Antigo", Preco = 100 };
            var updatedProduct = new Product { Id = 1, Nome = "Produto Atualizado", Preco = 150 };

            _productRepositoryMock.Setup(r => r.Get(productDto.Id)).ReturnsAsync(existingProduct);
            _mapperMock.Setup(m => m.Map(productDto, existingProduct)).Returns(updatedProduct);
            _mapperMock.Setup(m => m.Map<Product>(existingProduct)).Returns(updatedProduct);
            _productRepositoryMock.Setup(r => r.Update(updatedProduct)).Returns(Task.CompletedTask);

            // Act
            await _productService.UpdateProduct(productDto);

            // Assert
            _productRepositoryMock.Verify(r => r.Update(updatedProduct), Times.Once);
        }

        [Fact(DisplayName = "Atualizar Produto inexistente deve lançar exceção")]
        [Trait("Categoria", "Teste de Serviços Prdutos")]
        public async Task UpdateProduct_DeveLancarExcecao_QuandoProdutoNaoExistir()
        {
            // Arrange
            var productDto = new ProductDto { Id = 999, Nome = "Produto Atualizado", Preco = 150 };

            _productRepositoryMock.Setup(r => r.Get(productDto.Id)).ReturnsAsync((Product)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.UpdateProduct(productDto));
            Assert.Contains("Produto com ID 999 não foi encontrado", exception.Message);
        }
    }
}
