using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Model;
using Mango.Services.ProductAPI.Model.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IMapper mapper;
        ResponseDto responseDto;

        public ProductAPIController(AppDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            responseDto = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto GetAll()
        {
            try
            {
                IEnumerable<Product> resp = (IEnumerable<Product>)dbContext.Products.ToList();
                responseDto.Result = mapper.Map<IEnumerable<ProductDto>>(resp);

            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }
        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto GetById([FromRoute] int id)
        {
            try
            {
                var prod = dbContext.Products.FirstOrDefault(x => x.ProductId == id);
                responseDto.Result = mapper.Map<ProductDto>(prod);

            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }

        [HttpPost]
        public ResponseDto Post([FromBody] ProductDto productDto)
        {
            try
            {
                var product = mapper.Map<Product>(productDto);
                dbContext.Products.Add(product);
                dbContext.SaveChanges();
                responseDto.Result = mapper.Map<ProductDto>(productDto);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }

        [HttpPut]
        public ResponseDto Put([FromBody] ProductDto productDto) 
        {
            try
            {
                Product product = mapper.Map<Product>(productDto);
                dbContext.Products.Update(product);
                dbContext.SaveChanges();
                responseDto.Result = mapper.Map<ProductDto>(product);

            }
            catch (Exception ex)
            {
                responseDto.Message= ex.Message;
                responseDto.IsSuccess= false;
                
            }
            return responseDto;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDto Delete([FromRoute] int id) 
        {
            try
            {
                var product = dbContext.Products.FirstOrDefault(x => x.ProductId == id);
                if (product is not null)
                {
                    dbContext.Products.Remove(product);
                    dbContext.SaveChanges();
                }
                else
                {
                    responseDto.IsSuccess = false;
                }
                responseDto.Result = mapper.Map<ProductDto>(product);

            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = ex.Message;
            }
            return responseDto;
        }

    }
}