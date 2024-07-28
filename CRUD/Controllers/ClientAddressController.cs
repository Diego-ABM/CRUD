using CRUD.Models;
using CRUD.Models.CrudBD;
using CRUD.Services;
using CRUD.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientAddressController : ControllerBase
    {
        // Inyecciones
        private readonly ClientAddressService _clientAddressService;
        private readonly ClientAddressValidation _clientAddressValidation;

        public ClientAddressController(ClientAddressService clientAddressService, ClientAddressValidation clientAddressValidation)
        {
            _clientAddressService = clientAddressService;
            _clientAddressValidation = clientAddressValidation;
        }

        // Crea un cliente

        [Authorize]
        [HttpPost("Create")]
        public IActionResult Create([FromBody] ClientAddressModel address)
        {
            ResponseModel response = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = _clientAddressValidation.Create(address);
                if (validation.Success)
                {
                    response = _clientAddressService.Create(address);
                    if (response.Success)
                    {
                        response.Code = (int)HttpStatusCode.OK;

                        return Ok(response);
                    }
                    else
                    {
                        response.Code = (int)HttpStatusCode.Conflict;

                        return Conflict(response);
                    }
                }
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

        // Consulta un cliente por su numero de identificación
        [Authorize]
        [HttpGet("Read/{idClient}")]
        public IActionResult Read(int idClient)
        {
            ResponseModel response = new();

            try
            {
                // Valida si el numero de identificación, cumple con el formato correcto.
                ValidationModel validation = _clientAddressValidation.ReadOrDelete(idClient);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Conuslta el numero de identificación en BD
                    response = _clientAddressService.Read(idClient);

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
        [HttpPut("Update")]
        public IActionResult Update([FromBody] ClientAddressModel address)
        {
            ResponseModel response = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = _clientAddressValidation.Update(address);
                if (validation.Success)
                {
                    response = _clientAddressService.Update(address);
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

        // Crea un cliente
        [Authorize]
        [HttpDelete("Delete/{idClient}")]
        public IActionResult Delete(int idClient)
        {

            ResponseModel response = new();

            try
            {
                // Valida si el numero de identificación, cumple con el formato correcto.
                ValidationModel validation = _clientAddressValidation.ReadOrDelete(idClient);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Elimina el cliente y todo lo realcionado a el en BD
                    response = _clientAddressService.Delete(idClient);

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
