using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService authService;
        private readonly ITokenProvider tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            this.authService = authService;
            this.tokenProvider = tokenProvider;
        }
        [HttpGet]
        public async Task<ActionResult> Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpGet]
        public async Task<ActionResult> Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem { Text = SD.RoleAdmin,Value =SD.RoleAdmin},
                new SelectListItem { Text = SD.RoleCustomer, Value = SD.RoleCustomer}
            };
            ViewBag.RoleList = roleList;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register( [FromBody] RegistrationRequestDto registrationRequestDto)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            ResponseDTO assignRole = new ResponseDTO();
            var response = await authService.RegisterAsync(registrationRequestDto);
            if(response is not null && response.IsSuccess)
            {
                if(string.IsNullOrEmpty(registrationRequestDto.Role))
                {
                    registrationRequestDto.Role = SD.RoleCustomer;
                }
                assignRole= await authService.AssignRoleAsync(registrationRequestDto);
                if(assignRole is not null & assignRole.IsSuccess) 
                {
                    TempData["success"] = "Registration successful";
                    return RedirectToAction(nameof(Login));
                
                }
            }
            else
            {
                TempData["error"] = response.Message;
                return RedirectToAction(nameof(Login));

            }
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem { Text = SD.RoleAdmin,Value =SD.RoleAdmin},
                new SelectListItem { Text = SD.RoleCustomer, Value = SD.RoleCustomer}
            };
            ViewBag.RoleList = roleList;
            return View(registrationRequestDto);
                            
        }
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            var response = await authService.LoginAsync(loginRequestDto);
            if (response is not null && response.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>
                        (Convert.ToString(responseDTO.Result));
                await SignInUser(loginResponseDto);
                tokenProvider.SetToken(loginResponseDto.Token);
                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("Customerror", responseDTO.Message);
                return View(loginRequestDto);
            }
        }
        public async Task<ActionResult> Logout()
        {
           await HttpContext.SignOutAsync();
           tokenProvider.ClearToken();
           return RedirectToAction("Index","Home");
        }
        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));


            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));



            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }


    }
}
