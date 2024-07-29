using CRUD.Models;
using CRUD.Models.CrudBD;
using CRUD.Services;
using CRUD.Validations.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        // Variables
        private readonly IUserService _userService;
        private readonly IUserValidation _userValidation;

        // Constructor
        public UserController(IUserService clientService, IUserValidation userValidation)
        {
            _userService = clientService;
            _userValidation = userValidation;
        }

        // Servicios
        [Authorize]
        [HttpPost("CreateAsync")]
        public async Task<IActionResult> CreateAsync([FromBody] UserModel user)
        {
            ResponseModel response = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = await _userValidation.CreateAsync(user);

                // Validaciones superadas
                if (validation.Success)
                {
                    // Intenta Crear en BD
                    response = await _userService.CreateAsync(user);

                    // Creado exitoso
                    if (response.Success)
                    {
                        response.Code = (int)HttpStatusCode.OK;
                        return Ok(response);
                    }
                    // Fallo al crear
                    else
                    {
                        response.Code = (int)HttpStatusCode.Conflict;
                        return Conflict(response);
                    }
                }
                // No supera las validaciones
                else
                {
                    response.Code = (int)HttpStatusCode.BadRequest;
                    response.Message = validation.Message;
                    response.RequestErros = validation.Erros;

                    return BadRequest(response);

                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize]
        [HttpGet("ReadAsync/{email}")]
        public async Task<IActionResult> ReadAsync(string email)
        {
            ResponseModel response = new();

            try
            {
                // Valida si el numero de identificación, cumple con el formato correcto.
                ValidationModel validation = await _userValidation.ReadOrDeleteAsync(email);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Conuslta el numero de identificación en BD
                    response = await _userService.ReadAsync(email);

                    //Si encontro un cliente
                    if (response.Success)
                    {
                        // Seteamos los datos para que el servicio responda
                        response.Code = (int)HttpStatusCode.OK;
                        return Ok(response);
                    }
                    // No encontrado
                    else
                    {
                        // Seteamos los datos para que el servicio responda
                        response.Code = (int)HttpStatusCode.NotFound;
                        return NotFound(response);
                    }

                }
                // No cumple con el formato requerido
                else
                {
                    // Seteamos los datos para que el servicio responda 
                    response.Code = (int)HttpStatusCode.BadRequest;
                    response.Success = false;
                    response.Message = validation.Message;
                    response.RequestErros = validation.Erros;

                    return BadRequest(response);
                }

            }
            catch (Exception ex)
            {
                // En caso de alguna excepción no controlada
                return BadRequest(ex.Message);
            }

        }

        [Authorize]
        [HttpPut("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] UserModel user)
        {
            ResponseModel response = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = await _userValidation.UpdateAsync(user);

                // Supera las validaciones
                if (validation.Success)
                {
                    // Intenta actualizar
                    response = await _userService.UpdateAsync(user);

                    // Actualizacion exitosa
                    if (response.Success)
                    {
                        response.Code = (int)HttpStatusCode.OK;
                        return Ok(response);
                    }
                    // Fallo la actualizacion
                    else
                    {
                        response.Code = (int)HttpStatusCode.InternalServerError;
                        return BadRequest(response);
                    }
                }
                // No supera las validaciones
                else
                {
                    response.Code = (int)HttpStatusCode.BadRequest;
                    response.Message = validation.Message;
                    response.RequestErros = validation.Erros;

                    return BadRequest(response);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize]
        [HttpDelete("DeleteAsync/{email}")]
        public async Task<IActionResult> DeleteAsync(string email)
        {

            ResponseModel response = new();

            try
            {
                // Valida si el numero de identificación, cumple con el formato correcto.
                ValidationModel validation = await _userValidation.ReadOrDeleteAsync(email);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Elimina el cliente y todo lo realcionado a el en BD
                    response = await _userService.DeleteAsync(email);

                    //Si el resultado es exitoso
                    if (response.Success)
                    {
                        // Seteamos los datos para que el servicio responda
                        response.Code = (int)HttpStatusCode.OK;
                        return Ok(response);
                    }
                    // No encontrado
                    else
                    {
                        // Seteamos los datos para que el servicio responda
                        response.Code = (int)HttpStatusCode.NotFound;
                        return NotFound(response);
                    }

                }
                // No cumple con el formato requerido
                else
                {
                    // Seteamos los datos para que el servicio responda 
                    response.Success = false;
                    response.Message = validation.Message;
                    response.RequestErros = validation.Erros;

                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                // En caso de alguna excepción no controlada
                return BadRequest(ex.Message);
            }

        }


    }
}
