using AutoMapper;
using Mango.Services.ProductAPI.Model;
using Mango.Services.ProductAPI.Model.DTO;

namespace Mango.Services.ProductAPI
{
    public class MappingConfig:Profile
    {
        public MappingConfig() 
        {
            CreateMap<ProductDto, Product>().ReverseMap();
        }

    }
}
