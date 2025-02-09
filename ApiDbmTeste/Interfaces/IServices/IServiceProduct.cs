using ApiDbmTeste.Data.Dtos;

namespace ApiDbmTeste.Interfaces.IServices
{
    public interface IServiceProduct
    {
        Task AddProduct(ProductDto product);
        Task UpdateProduct(ProductDto product);
        Task<ProductDto> GetProduct(int id);
        Task<IEnumerable<ProductDto>> GetAllProduct();
        Task DeleteProduct(int id);
    }
}
