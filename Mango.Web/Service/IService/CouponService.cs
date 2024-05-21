using Mango.Web.Models;
using Mango.Web.Utility;

namespace Mango.Web.Service.IService
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService baseService;

        public  CouponService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async  Task<ResponseDTO?> CreateCouponAsync(CouponDto couponDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.APIType.POST,
                Data = couponDto,
                Url = SD.CouponAPIBase + "/api/coupon/" 

            });
        }

        public async Task<ResponseDTO?> DeleteCouponAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.APIType.DELETE,
                Url = SD.CouponAPIBase + "/api/coupon/" +id

            });
        }

        public async Task<ResponseDTO?> GetAllCouponsAsync()
        {
            return await baseService.SendAsync(new RequestDto()
            {
               ApiType= Utility.SD.APIType.GET,
               Url = SD.CouponAPIBase + "/api/coupon",
            });
        }

        public async Task<ResponseDTO?> GetCouponAsync(string CouponCode)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = Utility.SD.APIType.GET,
                Url = SD.CouponAPIBase + "/api/coupon/GetByCode/" + CouponCode
            });
        }

        public async Task<ResponseDTO?> GetCouponsByIdAsync(int id)
        {
            return await baseService.SendAsync(new RequestDto
            {
                ApiType = Utility.SD.APIType.GET,
                Url = SD.CouponAPIBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDTO?> UpdateCouponAsync(CouponDto couponDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.APIType.PUT,
                Data = couponDto,
                Url = SD.CouponAPIBase + "/api/coupon/"

            });
        }
    }
}
