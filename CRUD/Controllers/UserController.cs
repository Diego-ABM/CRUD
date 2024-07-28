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
    public class UserController : ControllerBase
    {
        // Inyecciones
        private readonly UserService _userService;
        private readonly UserValidation _userValidation;

        public UserController(UserService clientService, UserValidation userValidation)
        {
            _userService = clientService;
            _userValidation = userValidation;
        }

        // Crea un cliente

        [Authorize]
        [HttpPost("Create")]
        public IActionResult Create([FromBody] UserModel user)
        {
            ResponseControllerModel responseService = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = _userValidation.Create(user);
                if (validation.Success)
                {
                    ResponseControllerModel InsertResult = _userService.Create(user);
                    if (InsertResult.Success)
                    {
                        responseService.Code = (int)HttpStatusCode.OK;
                        responseService.Message = InsertResult.Message;
                        responseService.Success = true;

                        return Ok(responseService);
                    }
                    else
                    {
                        responseService.Code = (int)HttpStatusCode.Conflict;
                        responseService.Message = InsertResult.Message;
                        responseService.Success = false;

                        return Conflict(responseService);
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

        // Consulta un cliente por su numero de identificación
        [Authorize]
        [HttpGet("Read/{email}")]
        public IActionResult Read(string email)
        {
            ResponseControllerModel responseModel = new();

            try
            {
                // Valida si el numero de identificación, cumple con el formato correcto.
                ValidationModel validation = _userValidation.ReadOrDelete(email);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Conuslta el numero de identificación en BD
                    ResponseControllerModel result = _userService.Read(email);

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
                    responseModel.Code = (int)HttpStatusCode.BadRequest;
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

        // Crea un cliente
        [Authorize]
        [HttpPut("Update")]
        public IActionResult Update([FromBody] UserModel user)
        {
            ResponseControllerModel responseService = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = _userValidation.Update(user);
                if (validation.Success)
                {
                    ResponseControllerModel InsertResult = _userService.Update(user);
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

        // Crea un cliente
        [Authorize]
        [HttpDelete("Delete/{email}")]
        public IActionResult Delete(string email)
        {

            ResponseControllerModel responseModel = new();

            try
            {
                // Valida si el numero de identificación, cumple con el formato correcto.
                ValidationModel validation = _userValidation.ReadOrDelete(email);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Elimina el cliente y todo lo realcionado a el en BD
                    ResponseControllerModel result = _userService.Delete(email);

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
