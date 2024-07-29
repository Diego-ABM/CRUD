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
        // Variables
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly IAuthValidation _authValidation;

        // Cosntructor
        public AuthController(IConfiguration configuration, IAuthValidation authValidation, IAuthService authService)
        {
            _configuration = configuration;
            _authValidation = authValidation;
            _authService = authService;
        }

        // Servicios
        [HttpPost("LoginAsync")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel login)
        {
            ResponseModel response = new();

            try
            {
                // Valida la estructura del request LoginModel
                ValidationModel validation = await _authValidation.LoginAsync(login);

                if (validation.Success)
                {
                    // Intenta Loguear al usuario
                    response = await _authService.LoginAsync(login);

                    // Autenticación exitosa
                    if (response.Success)
                    {
                        // Genera un JWT y lo retorna al cliente para que pueda utilizar los servicios
                        string token = GenerateJwtToken(login.CorreoElectronico);
                        return Ok(token);
                    }
                    // Autenticación Fallida
                    else
                    {
                        // Regresa la respuesta del servicio
                        return BadRequest(response);
                    }
                }
                else
                {
                    // No supera las validaciones
                    response.Code = (int)HttpStatusCode.BadRequest;
                    response.Success = validation.Success;
                    response.Message = validation.Message;
                    response.RequestErros = validation.Erros;

                    return BadRequest(response);
                }


            }
            catch (Exception ex)
            {
                // En caso de una excepcion no controlada
                return StatusCode(500, ex.Message);
            }

        }

        // Funciones
        private string GenerateJwtToken(string username)
        {
            // Definir los claims (reclamaciones) que estarán en el token JWT.
            // Aquí se incluye el nombre de usuario y un identificador único (JTI).
            Claim[] claims =
            {
                new (JwtRegisteredClaimNames.Sub, username), // Reclamo con el nombre de usuario
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Reclamo con un identificador único
            };

            // Crear una clave de seguridad utilizando la clave configurada en la configuración de la aplicación.
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            // Crear las credenciales de firma usando la clave de seguridad y el algoritmo HMAC SHA-256.
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            // Crear el token JWT especificando el emisor, audiencia, claims, tiempo de expiración y credenciales de firma.
            JwtSecurityToken token = new(
                issuer: _configuration["Jwt:Issuer"], // Emisor del token
                audience: _configuration["Jwt:Audience"], // Audiencia del token
                claims: claims, // Reclamos incluidos en el token
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])), // Tiempo de expiración del token
                signingCredentials: creds // Credenciales de firma
            );

            // Generar el token JWT y devolverlo como una cadena.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

}
