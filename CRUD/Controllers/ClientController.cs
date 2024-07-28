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
        // Inyecciones
        private readonly IClientService _clientService;
        private readonly IClientValidation _clientValidation;

        public ClientController(IClientService clientService, IClientValidation clientValidation)
        {
            _clientService = clientService;
            _clientValidation = clientValidation;
        }

        // Crea un cliente
        [Authorize]
        [HttpPost("CreateAsync")]
        public async Task<IActionResult> CreateAsync([FromBody] ClientModel client)
        {
            ResponseModel responseService = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = await _clientValidation.CreateAsync(client);
                if (validation.Success)
                {
                    ResponseModel InsertResult = await _clientService.CreateAsync(client);
                    if (InsertResult.Success)
                    {
                        responseService.Code = (int)HttpStatusCode.OK;
                        responseService.Message = InsertResult.Message;
                        responseService.Success = true;

                        return Ok(responseService);
                    }
                    else
                    {
                        responseService.Code = (int)HttpStatusCode.InternalServerError;
                        responseService.Message = InsertResult.Message;
                        responseService.Success = false;

                        return BadRequest(responseService);
                    }
                }
                else
                {
                    responseService.Code = (int)HttpStatusCode.BadRequest;
                    responseService.Message = validation.Message;
                    responseService.RequestErros = validation.Erros.ToDictionary();

                    return BadRequest(responseService);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }

        }

        // Consulta un cliente por su numero de identificación
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

        // Crea un cliente.
        [Authorize]
        [HttpPut("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] ClientModel client)
        {
            ResponseModel responseService = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = await _clientValidation.UpdateAsync(client);
                if (validation.Success)
                {
                    ResponseModel InsertResult = await _clientService.UpdateAsync(client);
                    if (InsertResult.Success)
                    {
                        responseService.Code = (int)HttpStatusCode.OK;
                        responseService.Message = InsertResult.Message;
                        responseService.Success = true;

                        return Ok(responseService);
                    }
                    else
                    {
                        responseService.Code = (int)HttpStatusCode.InternalServerError;
                        responseService.Message = InsertResult.Message;
                        responseService.Success = false;

                        return BadRequest(responseService);
                    }
                }
                else
                {
                    responseService.Code = (int)HttpStatusCode.BadRequest;
                    responseService.Message = validation.Message;
                    responseService.RequestErros = validation.Erros;

                    return BadRequest(responseService);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }

        }

        // Elimina un cliente
        [Authorize]
        [HttpDelete("DeleteAsync/{identificationNumber}")]
        public async Task<IActionResult> DeleteAsync(string identificationNumber)
        {

            ResponseModel responseModel = new();

            try
            {
                // Valida si el numero de identificación, cumple con el formato correcto.
                ValidationModel validation = await _clientValidation.ReadOrDeleteAsync(identificationNumber);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Elimina el cliente y todo lo realcionado a el en BD
                    ResponseModel result = await _clientService.DeleteAsync(identificationNumber);

                    //Si el resultado es exitoso
                    if (result.Success)
                    {
                        // Seteamos los datos para que el servicio responda
                        responseModel.Code = (int)HttpStatusCode.OK;
                        responseModel.Message = result.Message;
                        responseModel.Success = result.Success;
                        responseModel.Data = result.Data;

                        return Ok(responseModel);
                    }
                    // No encontrado
                    else
                    {
                        // Seteamos los datos para que el servicio responda
                        responseModel.Code = (int)HttpStatusCode.NotFound;
                        responseModel.Message = result.Message;

                        return NotFound(responseModel);
                    }

                }
                // No cumple con el formato requerido
                else
                {
                    // Seteamos los datos para que el servicio responda 
                    responseModel.Success = false;
                    responseModel.Message = validation.Message;
                    responseModel.RequestErros = validation.Erros;

                    return BadRequest(responseModel);
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
