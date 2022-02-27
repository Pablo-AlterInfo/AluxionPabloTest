using AluxionTest.Models;
using AluxionTest.Models.Dtos;
using AluxionTest.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AluxionTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailServiceRepository _mailServiceRepository;
        protected ResponseDto _response;
        public UsersController(IUserRepository userRepositorio, IMailServiceRepository mailServiceRepository)

        {
            _userRepository = userRepositorio;
            _response = new ResponseDto();
            _mailServiceRepository = mailServiceRepository;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto model)
        {

            if (ModelState.IsValid)
            {
                var result = await _userRepository.RegisterUserAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result); //200
                }
                return BadRequest(result); //400
            }
            return BadRequest("Algunas propiedades no son válidas"); //400
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepository.LoginUserAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("Algunas propiedades no son válidas"); //400
        }


        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return NotFound();
            }
            var result = await _userRepository.ForgetPasswordAsync(email);
            if (result.IsSuccess)
            {
                Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepository.ResetPasswordAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("Algunas propiedades no son validas");
        }

    }
}
