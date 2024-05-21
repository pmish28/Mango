using AutoMapper;
using Mango.Services.AuthAPI.Model.DTO;
using Mango.Services.AuthAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly AuthService _authService;
        private ResponseDTO _responseDTO;
        //private IMapper _mapper;
        public AuthAPIController(AuthService authService )
        {
            this._authService = authService;
            this._responseDTO = new();

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            
            var errormsg = await _authService.Register(registrationRequestDto);
            if (string.IsNullOrEmpty(errormsg))
            {
                _responseDTO.IsSuccess = true;
                _responseDTO.Result = errormsg;
                return BadRequest(_responseDTO);
            }
            
            return Ok(_responseDTO);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {

            var loginResponseDto = await _authService.Login(loginRequestDto);
            if (loginResponseDto.User is null)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Result = "User name password is incorrect";
                return BadRequest(_responseDTO);
            }
            _responseDTO.Result = loginResponseDto;
            return Ok(_responseDTO);

        }


        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto registrationRequestDto)
        {

            var assignRolesucessful = await _authService.AssignRole(registrationRequestDto.Email,registrationRequestDto.Role.ToUpper());
            if (!assignRolesucessful)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Result = "User name password is incorrect";
                return BadRequest(_responseDTO);
            }
            
            return Ok(_responseDTO);

        }


    }
}
