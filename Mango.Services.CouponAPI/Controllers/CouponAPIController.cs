using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private ResponseDTO _responseDTO;
        private IMapper _mapper;
        public CouponAPIController(AppDbContext appDbContext, IMapper mapper)
        {
            this._appDbContext = appDbContext;
            _mapper = mapper;
            _responseDTO = new ResponseDTO();
        }

        [HttpGet]
        public ResponseDTO Get() 
        {
            try
            {
                IEnumerable<Coupon> objList = _appDbContext.Coupons.ToList();
                _responseDTO.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);
            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;
                
            }
            return _responseDTO;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDTO Get(int id)
        {
            try
            {
               Coupon obj =  _appDbContext.Coupons.FirstOrDefault(x => x.CouponId == id);
                _responseDTO.Result =  _mapper.Map<CouponDto>(obj);

            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess =false;
                _responseDTO.Message = ex.Message;

            }
            return _responseDTO;
        }

        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDTO GetByCode(string code)
        {
            try
            {
                Coupon obj = _appDbContext.Coupons.FirstOrDefault(x => x.CouponCode.ToLower() == code.ToLower());
                if(obj is null)
                {
                    _responseDTO.IsSuccess = false;
                }
                _responseDTO.Result = _mapper.Map<CouponDto>(obj);

            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;

            }
            return _responseDTO;
        }

        [HttpPost]
        public ResponseDTO Post([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon obj =  _mapper.Map<Coupon>(couponDto);

                _appDbContext.Coupons.Add(obj);
                _appDbContext.SaveChanges();
                
                _responseDTO.Result = _mapper.Map<CouponDto>(obj);

            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;

            }
            return _responseDTO;
        }

        [HttpPut]
        public ResponseDTO Put([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon obj = _mapper.Map<Coupon>(couponDto);

                _appDbContext.Coupons.Update(obj);
                _appDbContext.SaveChanges();

                _responseDTO.Result = _mapper.Map<CouponDto>(obj);

            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;

            }
            return _responseDTO;
        }


        [HttpDelete]
        [Route("{id:int}")]
        public ResponseDTO Delete([FromRoute] int id)
        {
            try
            {
                Coupon coupon = _appDbContext.Coupons.FirstOrDefault(x => x.CouponId == id);
                if (coupon == null)
                {
                    _responseDTO.IsSuccess = false;
                }
                _appDbContext.Coupons.Remove(coupon);
                _appDbContext.SaveChanges();

                _responseDTO.Result = _mapper.Map<CouponDto>(coupon);

            }
            catch (Exception ex)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = ex.Message;

            }
            return _responseDTO;
        }
    }
}
