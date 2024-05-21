using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDTO?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDTO?> RegisterAsync(RegistrationRequestDto registerrequestDto);
        Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDto registerrequestDto);
    }
}
