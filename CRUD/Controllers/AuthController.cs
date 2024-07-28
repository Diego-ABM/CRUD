using CRUD.Models;
using CRUD.Services;
using CRUD.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;
        private readonly AuthValidation _authValidation;

        public AuthController(IConfiguration configuration, AuthValidation authValidation, AuthService authService)
        {
            _configuration = configuration;
            _authValidation = authValidation;
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            ResponseModel response = new();

            try
            {
                ValidationModel validation = _authValidation.Login(login);

                if (validation.Success)
                {
                    response = _authService.Login(login);

                    if (response.Success)
                    {

                        string token = GenerateJwtToken(login.CorreoElectronico);
                        return Ok(token);

                    }
                    else
                    {
                        return BadRequest(response);
                    }
                }
                else
                {
                    response.Code = (int)HttpStatusCode.BadRequest;
                    response.Success = validation.Success;
                    response.Message = validation.Message;
                    response.RequestErros = validation.Erros;

                    return BadRequest(response);
                }


            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }



        }

        private string GenerateJwtToken(string username)
        {
            Claim[] claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
