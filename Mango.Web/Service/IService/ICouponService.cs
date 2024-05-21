using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDTO?> GetCouponAsync(string CouponCode);
        Task<ResponseDTO?> GetAllCouponsAsync();
        Task<ResponseDTO?> GetCouponsByIdAsync(int id);
        Task<ResponseDTO?> UpdateCouponAsync(CouponDto couponDto);
        Task<ResponseDTO?> CreateCouponAsync(CouponDto couponDto);
        Task<ResponseDTO?> DeleteCouponAsync(int id);

    }
}
