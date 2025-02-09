using FluentValidation;
using ApiDbmTeste.Data.Dtos;
using ApiDbmTeste.Interfaces.IRepositories;

namespace ApiDbmTeste.FluentValidations;
public class ProductValidator : AbstractValidator<ProductDto>
{
    private readonly IProductRepository _productRepository;

    public ProductValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;

        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.")
            .MustAsync(async (nome, cancellation) =>
            {
                var products = await _productRepository.GetAll(); 
                return !products.Any(p => p.Nome == nome); 
            })
            .WithMessage("Nome já existe.");

        RuleFor(p => p.Preco)
            .NotNull().WithMessage("Preço não pode ser nulo.")
            .GreaterThan(0).WithMessage("Preço deve ser maior que zero.");
    }
}
