using ApiDbmTeste.Data.Context;
using ApiDbmTeste.Data.Entities;
using ApiDbmTeste.Interfaces.IRepositories;

namespace ApiDbmTeste.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(MyContext myContext) : base(myContext)
        {
        }
    }
}
