using AluxionTest.Data;
using AluxionTest.Models;
using AluxionTest.Models.Dtos;
using AluxionTest.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AluxionTest.Repositories
{
    public class UserRepository : IUserRepository
    {

        private UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IMailServiceRepository _mailServiceRepository;
        public UserRepository(ApplicationDbContext db, IMapper mapper, IConfiguration configuration, UserManager<IdentityUser> userManager, IMailServiceRepository mailServiceRepository)
        {
            _db = db;
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
            _mailServiceRepository = mailServiceRepository;
        }

        public async Task<ResponseDto> RegisterUserAsync(RegisterDto model)
        {

            if (model == null)
            {
                throw new NullReferenceException("Modelo de Registro nulo");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return new ResponseDto
                {
                    DisplayMessage = "Las contraseñas no coinciden",
                    IsSuccess = false,
                };
            }

            var identityUSer = new IdentityUser
            {
                Email = model.Email,
                UserName = model.UserName,
            };


            var result = await _userManager.CreateAsync(identityUSer, model.Password);

            if (result.Succeeded)
            {
                return new ResponseDto
                {
                    DisplayMessage = "Usuario creado de manera exitosa",
                    IsSuccess = true,
                };
            }

            return new ResponseDto
            {
                DisplayMessage = "No se creó el usuario",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };

        }

        public async Task<ResponseDto> LoginUserAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new ResponseDto
                {
                    DisplayMessage = "Usuario no existe",
                    IsSuccess = false,
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                return new ResponseDto
                {
                    DisplayMessage = "Contraseña Invalida",
                    IsSuccess = false,
                };
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.
                GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new ResponseDto
            {
                DisplayMessage = tokenHandler.WriteToken(token),
                IsSuccess = true,
                ExpireDate = token.ValidTo
            };
        }

        public async Task<ResponseDto> ForgetPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    DisplayMessage = "No existe un usuario asociado con ese correo."
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);
            await _mailServiceRepository.SendEmailAsync(email, "Reestablecer Contraseña", "<h1> Siga las Instrucciones para Reestablecer la contraseña</h1>" +
                "<p>Para Reestablecer la contraseña, diríjase al Swagger UI de este proyecto de prueba, diríjase al Endpoint 'ResetPassword' y coloque la siguiente llave como parametro de entrada: </p>" +
                validToken + "<p>Junto a ese Token, introduzca su nueva contraseña en los campos correspondientes.</p>");
            return new ResponseDto
            {
                IsSuccess = true,
                DisplayMessage = "El Token de Reestablecer Contraseña ha sido enviado al correo: " + email,
            };
        }

        public async Task<ResponseDto> ResetPasswordAsync(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);


            if (user == null)
            {
                return new ResponseDto
                {
                    DisplayMessage = "Usuario no existe",
                    IsSuccess = false,
                };
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                return new ResponseDto
                {
                    DisplayMessage = "Contraseñas no Coinciden",
                    IsSuccess = false
                };
            }

            var decodedtoken = WebEncoders.Base64UrlDecode(model.Token);
            var normalToken = Encoding.UTF8.GetString(decodedtoken);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);

            if (result.Succeeded)
            {
                return new ResponseDto
                {
                    DisplayMessage = "Contraseña Actualizada con Exito",
                    IsSuccess = true
                };
            }
            return new ResponseDto
            {
                DisplayMessage = "Algo salió mal",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }
    }
}
