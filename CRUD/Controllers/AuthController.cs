using CRUD.Models;
using CRUD.Services.Interfaces;
using CRUD.Validations.Interfaces;
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
        private readonly IAuthService _authService;
        private readonly IAuthValidation _authValidation;

        public AuthController(IConfiguration configuration, IAuthValidation authValidation, IAuthService authService)
        {
            _configuration = configuration;
            _authValidation = authValidation;
            _authService = authService;
        }

        [HttpPost("LoginAsync")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel login)
        {
            ResponseModel response = new();

            try
            {
                ValidationModel validation = await _authValidation.LoginAsync(login);

                if (validation.Success)
                {
                    response = await _authService.LoginAsync(login);

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
            Claim[] claims =
            [
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
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
