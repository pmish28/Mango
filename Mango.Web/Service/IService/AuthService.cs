using Mango.Web.Models;
using Mango.Web.Utility;

namespace Mango.Web.Service.IService
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService baseService;

        public AuthService(IBaseService baseService)
        {
            this.baseService = baseService;
        }
        public async Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDto registerrequestDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.APIType.POST,
                Data = registerrequestDto,
                Url = SD.CouponAPIBase + "/api/auth/AssignRole"

            },withBearer:false);
        }

        public async  Task<ResponseDTO?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.APIType.POST,
                Data = loginRequestDto,
                Url = SD.CouponAPIBase + "/api/auth/login"

            }, withBearer: false);
        }

        public async  Task<ResponseDTO?> RegisterAsync(RegistrationRequestDto registerrequestDto)
        {
            return await baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.APIType.POST,
                Data = registerrequestDto,
                Url = SD.CouponAPIBase + "/api/auth/register"

            }, withBearer: false);
        }


    }
}
