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
    public class ClientEmailController : ControllerBase
    {
        // Variables
        private readonly IClientEmailService _clientEmailService;
        private readonly IClientEmailValidation _clientEmailValidation;

        // Constructor
        public ClientEmailController(IClientEmailService clientEmailService, IClientEmailValidation clientEmailValidation)
        {
            _clientEmailService = clientEmailService;
            _clientEmailValidation = clientEmailValidation;
        }

        // Servicios
        [Authorize]
        [HttpPost("CreateAsync")]
        public async Task<IActionResult> CreateAsync([FromBody] ClientEmailModel email)
        {
            ResponseModel response = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = await _clientEmailValidation.CreateAsync(email);

                // Supera las validaciones
                if (validation.Success)
                {
                    // Intenta crear en BD
                    response = await _clientEmailService.CreateAsync(email);

                    // Creación exitosa
                    if (response.Success)
                    {
                        response.Code = (int)HttpStatusCode.OK;
                        return Ok(response);
                    }
                    // Creación fallida
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
        [HttpGet("ReadAsync/")]
        public async Task<IActionResult> ReadAsync(int idClient, string email = "")
        {

            ResponseModel response = new();
            ValidationModel validation;
            try
            {
                // Inicia las validaciones del Request
                // Valida el parametro opcional (email)
                if (string.IsNullOrEmpty(email))
                    // Valida solo el id
                    validation = await _clientEmailValidation.ReadOrDeleteAsync(idClient);
                else
                    // Valida id y email
                    validation = await _clientEmailValidation.ReadOrDeleteAsync(idClient, email);

                // Validaciones superadas
                if (validation.Success)
                {
                    // Valida el parametro ocpional
                    if (string.IsNullOrEmpty(email))
                        // En caso de que se diligencie solo el id se regresan todos los email asociados al cliente
                        response = await _clientEmailService.ReadAsync(idClient);
                    else
                        // En caso que se diligencien ambos regresa unicamente ese registro
                        response = await _clientEmailService.ReadAsync(idClient, email);

                    // Consulta exitosa
                    if (response.Success)
                    {
                        response.Code = (int)HttpStatusCode.OK;
                        return Ok(response);
                    }
                    // Consulta fallida
                    else
                    {
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

        // Crea un cliente
        [Authorize]
        [HttpPut("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] ClientEmailModel email)
        {
            ResponseModel response = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = await _clientEmailValidation.UpdateAsync(email);

                // Validaciones superadas
                if (validation.Success)
                {
                    // Intenta actualizar
                    response = await _clientEmailService.UpdateAsync(email);

                    // Actualizacion exitosa
                    if (response.Success)
                    {
                        response.Code = (int)HttpStatusCode.OK;
                        return Ok(response);
                    }
                    // Actualización fallida
                    else
                    {
                        response.Code = (int)HttpStatusCode.InternalServerError;
                        return BadRequest(response);
                    }
                }
                // No cumple con el formato esperado
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

        // Crea un cliente
        [Authorize]
        [HttpDelete("DeleteAsync/{idClient}")]
        public async Task<IActionResult> DeleteAsync(int idClient)
        {

            ResponseModel response = new();

            try
            {
                // Valida si el numero de identificación, cumple con el formato correcto.
                ValidationModel validation = await _clientEmailValidation.ReadOrDeleteAsync(idClient);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Elimina el cliente y todo lo realcionado a el en BD
                    response = await _clientEmailService.DeleteAsync(idClient);

                    //Si el resultado es exitoso
                    if (response.Success)
                    {

                        response.Code = (int)HttpStatusCode.OK;
                        return Ok(response);
                    }
                    // No encontrado
                    else
                    {
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
