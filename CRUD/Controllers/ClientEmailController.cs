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
    public class ClientEmailController : ControllerBase
    {
        // Inyecciones
        private readonly ClientEmailService _clientEmailService;
        private readonly ClientEmailValidation _clientEmailValidation;

        public ClientEmailController(ClientEmailService clientEmailService, ClientEmailValidation clientEmailValidation)
        {
            _clientEmailService = clientEmailService;
            _clientEmailValidation = clientEmailValidation;
        }

        // Crea un cliente
        [Authorize]
        [HttpPost("Create")]
        public IActionResult Create([FromBody] ClientEmailModel email)
        {
            ResponseModel response = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = _clientEmailValidation.Create(email);
                if (validation.Success)
                {
                    response = _clientEmailService.Create(email);
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
        [HttpGet("Read/")]
        public IActionResult Read([FromBody] int idClient, string email = "")
        {
            ResponseModel response = new();
            ValidationModel validation;
            try
            {
                // Valida el parametro ocpional
                if (string.IsNullOrEmpty(email))
                    validation = _clientEmailValidation.ReadOrDelete(idClient);
                else
                    validation = _clientEmailValidation.ReadOrDelete(idClient, email);


                // Validaciones superadas
                if (validation.Success)
                {
                    // Valida el parametro ocpional
                    if (string.IsNullOrEmpty(email))
                        response = _clientEmailService.Read(idClient);
                    else
                        response = _clientEmailService.Read(idClient, email);

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
        public IActionResult Update([FromBody] ClientEmailModel email)
        {
            ResponseModel response = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = _clientEmailValidation.Update(email);
                if (validation.Success)
                {
                    response = _clientEmailService.Update(email);
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
                ValidationModel validation = _clientEmailValidation.ReadOrDelete(idClient);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Elimina el cliente y todo lo realcionado a el en BD
                    response = _clientEmailService.Delete(idClient);

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
