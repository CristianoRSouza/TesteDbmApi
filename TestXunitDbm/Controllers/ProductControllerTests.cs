using ApiDbmTeste.AutoMapperConfig;
using ApiDbmTeste.Controllers;
using ApiDbmTeste.Data.Dtos;
using ApiDbmTeste.Data.Entities;
using ApiDbmTeste.Interfaces.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestXunitDbm.Fixtures;

namespace TestXunitDbm.Controllers
{
    [Collection(nameof(ProductCollection))]
    public class ProductControllerTests
    {
        private readonly Mock<IServiceProduct> _serviceMock;
        private readonly ProductController _controller;
        private readonly ProductTestFixture _productTestFixture;
        private readonly IMapper _mapper;

        public ProductControllerTests(ProductTestFixture productTestFixture)
        {
            _serviceMock = new Mock<IServiceProduct>();
            _controller = new ProductController(_serviceMock.Object);
            _productTestFixture = productTestFixture;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperConfig>(); 
            });

            _mapper = config.CreateMapper();
        }

        [Fact(DisplayName ="Busca todos os produtos")]
        [Trait("Categoria","Teste de produto")]
        public async Task GetAll_DeveRetornarListaDeProdutos()
        {
            // Arrange
            var produtos = _productTestFixture.GenerateListProduct();

            _serviceMock.Setup(s => s.GetAllProduct()).ReturnsAsync(_mapper.Map<List<ProductDto>>(produtos));

            // Act
            var resultado = await _controller.GetAll();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(20, resultado.Count());
        }

        [Fact(DisplayName = "Busca um produto")]
        [Trait("Categoria", "Teste de produto")]
        public async Task Get_DeveRetornarProduto_QuandoIdForValido()
        {
            // Arrange
            var produto = _productTestFixture.GenerateValidProduct();

            _serviceMock.Setup(s => s.GetProduct(1)).ReturnsAsync(_mapper.Map<ProductDto>(produto));

            // Act
            var resultado = await _controller.Get(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.True(resultado.Id > 0);
        }

        [Fact(DisplayName = "Cria um produto")]
        [Trait("Categoria", "Teste de produto")]
        public async Task Post_DeveRetornarOk_QuandoProdutoForValido()
        {
            // Arrange
            var produto = _productTestFixture.GenerateValidProduct();

            _serviceMock.Setup(s => s.AddProduct(_mapper.Map<ProductDto>(produto))).Returns(Task.CompletedTask);

            // Act
            var resultado = await _controller.Post(_mapper.Map<ProductDto>(produto)) as ObjectResult;

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(200, resultado.StatusCode);
            Assert.Equal("Produto cadastrado com sucesso!", resultado.Value);
        }

        [Fact(DisplayName = "Valida edição do produto")]
        [Trait("Categoria", "Teste de produto")]
        public async Task Put_DeveAtualizarProduto()
        {
            // Arrange
            var produto = _productTestFixture.GenerateValidProduct();

            _serviceMock.Setup(s => s.UpdateProduct(_mapper.Map<ProductDto>(produto))).Returns(Task.CompletedTask);

            // Act & Assert
            await _controller.Put(_mapper.Map<ProductDto>(produto));

            _serviceMock.Verify(s => s.UpdateProduct(It.IsAny<ProductDto>()), Times.Once);
        }

        [Fact(DisplayName = "Deleta um produto")]
        [Trait("Categoria", "Teste de produto")]
        public async Task Delete_DeveRemoverProduto()
        {
            // Arrange
            int produtoId = 1;

            _serviceMock.Setup(s => s.DeleteProduct(produtoId)).Returns(Task.CompletedTask);

            // Act
            await _controller.Delete(produtoId);

            // Assert
            _serviceMock.Verify(s => s.DeleteProduct(produtoId), Times.Once);
        }
    }
}
