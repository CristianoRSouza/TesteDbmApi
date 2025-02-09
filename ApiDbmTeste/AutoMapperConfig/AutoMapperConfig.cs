using ApiDbmTeste.Data.Dtos;
using ApiDbmTeste.Data.Entities;
using AutoMapper;

namespace ApiDbmTeste.AutoMapperConfig
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
        }
    }
}
