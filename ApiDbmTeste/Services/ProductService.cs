using ApiDbmTeste.Data.Dtos;
using ApiDbmTeste.Data.Entities;
using ApiDbmTeste.Interfaces.IRepositories;
using ApiDbmTeste.Interfaces.IServices;
using AutoMapper;
using FluentValidation;

namespace ApiDbmTeste.Services
{
    public class ProductService : IServiceProduct
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _Mapper;
        private readonly IValidator<ProductDto> _validator;

        public ProductService(IProductRepository productRepository, IMapper mapper, IValidator<ProductDto> validator)
        {
            _productRepository = productRepository;
            _Mapper = mapper;
            _validator = validator;
        }
        public async Task AddUser(ProductDto product)
        {
            var validationResult = await _validator.ValidateAsync(product);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            await _productRepository.Add(_Mapper.Map<Product>(product));
        }

        public async Task DeleteUser(int id)
        {
            await _productRepository.Delete(id);
        }

        public async Task<IEnumerable<ProductDto>> GetAllUser()
        {
            return _Mapper.Map<IEnumerable<ProductDto>>(await _productRepository.GetAll());
        }

        public async Task<ProductDto> GetUser(int id)
        {
            return _Mapper.Map<ProductDto>(await _productRepository.Get(id));
        }

        public async Task UpdateUser(ProductDto user)
        {
            var existingUser = await _productRepository.Get(user.Id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"Produto com ID {user.Id} não foi encontrado.");
            }

            _Mapper.Map(user, existingUser);


            await _productRepository.Update(_Mapper.Map<Product>(existingUser));
        }
    }
}
