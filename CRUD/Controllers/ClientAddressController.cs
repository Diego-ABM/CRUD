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
    public class ClientAddressController : ControllerBase
    {
        // Variables
        private readonly IClientAddressService _clientAddressService;
        private readonly IClientAddressValidation _clientAddressValidation;

        // Constructor
        public ClientAddressController(IClientAddressService clientAddressService, IClientAddressValidation clientAddressValidation)
        {
            _clientAddressService = clientAddressService;
            _clientAddressValidation = clientAddressValidation;
        }

        // Servicios
        [Authorize]
        [HttpPost("CreateAsync")]
        public async Task<IActionResult> CreateAsync([FromBody] ClientAddressModel address)
        {
            ResponseModel response = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = await _clientAddressValidation.CreateAsync(address);

                // Supera las validaciones
                if (validation.Success)
                {
                    // Inicia proceso de creación
                    response = await _clientAddressService.CreateAsync(address);

                    // Creado correctamente
                    if (response.Success)
                    {
                        response.Code = (int)HttpStatusCode.OK;
                        return Ok(response);
                    }
                    // No se puedo crear
                    else
                    {
                        response.Code = (int)HttpStatusCode.Conflict;
                        return Conflict(response);
                    }
                }
                // No supera las validaciones
                else
                {
                    // Seteamos los datos para que el servicio responda 
                    response.Code = (int)HttpStatusCode.BadRequest;
                    response.Message = validation.Message;
                    response.RequestErros = validation.Erros;

                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                // En caso de excepcion no controlada
                return StatusCode(500, ex.Message);
            }

        }

        [Authorize]
        [HttpGet("ReadAsync/{idClient}")]
        public async Task<IActionResult> ReadAsync(int idClient)
        {
            ResponseModel response = new();

            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = await _clientAddressValidation.ReadOrDeleteAsync(idClient);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Inicia consulta en BD
                    response = await _clientAddressService.ReadAsync(idClient);

                    //Si encontro un cliente
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
        public async Task<IActionResult> UpdateAsync([FromBody] ClientAddressModel address)
        {
            ResponseModel response = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = await _clientAddressValidation.UpdateAsync(address);
                if (validation.Success)
                {
                    // Incia proceso en BD
                    response = await _clientAddressService.UpdateAsync(address);

                    // Proceso exitoso
                    if (response.Success)
                    {
                        response.Code = (int)HttpStatusCode.OK;
                        return Ok(response);
                    }
                    else
                    {
                        response.Code = (int)HttpStatusCode.InternalServerError;
                        return BadRequest(response);
                    }
                }
                // No supera las validaciones
                else
                {
                    // Seteamos los valores
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
                ValidationModel validation = await _clientAddressValidation.ReadOrDeleteAsync(idClient);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Intenta eliminar en BD
                    response = await _clientAddressService.DeleteAsync(idClient);

                    //Si el resultado es exitoso
                    if (response.Success)
                    {
                        response.Code = (int)HttpStatusCode.OK;
                        return Ok(response);
                    }
                    // No encontrado o no se pudo eliminar
                    else
                    {
                        response.Code = (int)HttpStatusCode.NotFound;
                        return NotFound(response);
                    }

                }
                // No supera las validaciones
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
