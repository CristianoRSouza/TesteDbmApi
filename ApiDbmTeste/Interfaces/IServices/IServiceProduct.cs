using ApiDbmTeste.Data.Dtos;

namespace ApiDbmTeste.Interfaces.IServices
{
    public interface IServiceProduct
    {
        Task AddUser(ProductDto product);
        Task UpdateUser(ProductDto product);
        Task<ProductDto> GetUser(int id);
        Task<IEnumerable<ProductDto>> GetAllUser();
        Task DeleteUser(int id);
    }
}
