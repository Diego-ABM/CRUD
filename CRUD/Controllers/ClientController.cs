using CRUD.Models;
using CRUD.Models.bdCrud;
using CRUD.Services.Interfaces;
using CRUD.Validations;
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
        private readonly ClientValidation _clientValidation;

        public ClientController(IClientService clientService, ClientValidation clientValidation)
        {
            _clientService = clientService;
            _clientValidation = clientValidation;
        }

        // Crea un cliente
        [HttpPost("Create")]
        public IActionResult Create([FromBody] ClientModel client)
        {
            ResponseControllerModel responseService = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = _clientValidation.Create(client);
                if (validation.Success)
                {
                    ResponseControllerModel InsertResult = _clientService.Create(client);
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
                    responseService.ClientErros = validation.Erros;

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
        [HttpGet("Read/{identificationNumber}")]
        public IActionResult Read(string identificationNumber)
        {
            ResponseControllerModel responseModel = new();

            try
            {
                // Valida si el numero de identificación, cumple con el formato correcto.
                ValidationModel validation = _clientValidation.ReadOrDelete(identificationNumber);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Conuslta el numero de identificación en BD
                    ResponseControllerModel result = _clientService.Read(identificationNumber);

                    //Si encontro un cliente
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
                    responseModel.ClientErros = validation.Erros;

                    return BadRequest(responseModel);
                }

            }
            catch (Exception ex)
            {
                // En caso de alguna excepción no controlada
                return BadRequest(ex.Message);
            }

        }

        // Crea un cliente
        [HttpPut("Update")]
        public IActionResult Update([FromBody] ClientModel client)
        {
            ResponseControllerModel responseService = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = _clientValidation.Update(client);
                if (validation.Success)
                {
                    ResponseControllerModel InsertResult = _clientService.Update(client);
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
                    responseService.ClientErros = validation.Erros;

                    return BadRequest(responseService);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }

        }

        // Crea un cliente
        [HttpDelete("Delete/{identificationNumber}")]
        public IActionResult Delete(string identificationNumber)
        {

            ResponseControllerModel responseModel = new();

            try
            {
                // Valida si el numero de identificación, cumple con el formato correcto.
                ValidationModel validation = _clientValidation.ReadOrDelete(identificationNumber);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Elimina el cliente y todo lo realcionado a el en BD
                    ResponseControllerModel result = _clientService.Delete(identificationNumber);

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
                    responseModel.ClientErros = validation.Erros;

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
