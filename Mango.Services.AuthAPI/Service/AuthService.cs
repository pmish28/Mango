using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Model;
using Mango.Services.AuthAPI.Model.DTO;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(AppDbContext _appDbContext,UserManager<ApplicationUser> _userManager,
            RoleManager<IdentityRole> _roleManager, JWTTokenGenerator jWTTokenGenerator)
        {
            this._appDbContext = _appDbContext;
            this._userManager = _userManager;
            this._roleManager = _roleManager;
            JWTTokenGenerator = jWTTokenGenerator;
        }

        public JWTTokenGenerator JWTTokenGenerator { get; }

        public async Task<bool> AssignRole(string emailId, string roleName)
        {
            var user = _appDbContext.ApplicationUsers.FirstOrDefault(u =>
                           u.Email.ToLower() == emailId.ToLower());
            if(user != null)
            {
                if(!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create role if not exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                    
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
            
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user= _appDbContext.ApplicationUsers.FirstOrDefault(u=>
                u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user,loginRequestDto.Password);
            if(user == null || isValid == false )
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }
            //if user was fouhnd generate JWT token
            var roles = await _userManager.GetRolesAsync(user);
            var token  = JWTTokenGenerator.GenerateToken(user, roles);
            UserDto userDto = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber= user.PhoneNumber
            
            };
            LoginResponseDto loginresponseDto = new LoginResponseDto() 
            {
                User = userDto,
                Token = ""//token
            };

            return loginresponseDto;

        }

        public async Task<string> Register(RegistrationRequestDto registrationrequestDto)
        {
            ApplicationUser user = new()
            {
                UserName= registrationrequestDto.Email,
                Email= registrationrequestDto.Email,
                Name= registrationrequestDto.Name,
                NormalizedEmail= registrationrequestDto.Email.ToUpper(),
                PhoneNumber= registrationrequestDto.PhoneNumber
            };
            try 
            {
                var result = await _userManager.CreateAsync(user, registrationrequestDto.Password);
                if(result.Succeeded)
                {
                    var userToreturn = _appDbContext.ApplicationUsers.FirstOrDefault(u =>
                        u.UserName == registrationrequestDto.Email);
                    UserDto userDto = new()
                    {
                        Email = userToreturn.Email,
                        ID = userToreturn.Id,
                        Name = userToreturn.Name,
                        PhoneNumber = userToreturn.PhoneNumber
                    };
                    return "";
                }
                else 
                {
                    return result.Errors.FirstOrDefault().Description;
                }
;            
            }
            catch (Exception ex)             
            {
            
            }
            return "Error Encountered"; 

        }
    }
}
