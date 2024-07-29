using CRUD.Models;
using CRUD.Models.bdCrud;
using CRUD.Services.Interfaces;
using CRUD.Validations.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {
        // Variables
        private readonly IClientService _clientService;
        private readonly IClientValidation _clientValidation;

        // Constructor
        public ClientController(IClientService clientService, IClientValidation clientValidation)
        {
            _clientService = clientService;
            _clientValidation = clientValidation;
        }

        // Servicios
        [Authorize]
        [HttpPost("CreateAsync")]
        public async Task<IActionResult> CreateAsync([FromBody] ClientModel client)
        {
            ResponseModel response = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = await _clientValidation.CreateAsync(client);

                // Supera las validaciones
                if (validation.Success)
                {
                    // Intenta crear en BD
                    response = await _clientService.CreateAsync(client);

                    // Creación exitosa
                    if (response.Success)
                    {
                        response.Code = (int)HttpStatusCode.OK;
                        return Ok(response);
                    }
                    // Creación fallida
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
                    response.RequestErros = validation.Erros.ToDictionary();

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
        [HttpGet("ReadAsync/{identificationNumber}")]
        public async Task<IActionResult> ReadAsync(string identificationNumber)
        {
            ResponseModel response = new();

            try
            {
                // Valida si el numero de identificación, cumple con el formato correcto.
                ValidationModel validation = await _clientValidation.ReadOrDeleteAsync(identificationNumber);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Conuslta el numero de identificación en BD
                    response = await _clientService.ReadAsync(identificationNumber);

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
        public async Task<IActionResult> UpdateAsync([FromBody] ClientModel client)
        {
            ResponseModel response = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = await _clientValidation.UpdateAsync(client);

                // Supera las validaciones
                if (validation.Success)
                {
                    // Intenta actualizar
                    response = await _clientService.UpdateAsync(client);

                    // Actualización correcta
                    if (response.Success)
                    {
                        response.Code = (int)HttpStatusCode.OK;
                        return Ok(response);
                    }
                    // Fallo la actualización
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
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize]
        [HttpDelete("DeleteAsync/{identificationNumber}")]
        public async Task<IActionResult> DeleteAsync(string identificationNumber)
        {

            ResponseModel response = new();

            try
            {
                // Valida si el numero de identificación, cumple con el formato correcto.
                ValidationModel validation = await _clientValidation.ReadOrDeleteAsync(identificationNumber);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Elimina el cliente y todo lo realcionado a el en BD
                    response = await _clientService.DeleteAsync(identificationNumber);

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
