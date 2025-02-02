﻿using CRUD.Models;
using CRUD.Models.CrudBD;
using CRUD.Services.Interfaces;
using CRUD.Validations.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientContactController : ControllerBase
    {
        // Variables
        private readonly IClientContactService _contactService;
        private readonly IClientContactValidation _contactValidation;

        // Constructor
        public ClientContactController(IClientContactService contactService, IClientContactValidation clientContact)
        {
            _contactService = contactService;
            _contactValidation = clientContact;
        }

        // Servicios
        [Authorize]
        [HttpPost("CreateAsync")]
        public async Task<IActionResult> CreateAsync([FromBody] ClientContactModel contact)
        {
            ResponseModel response = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = await _contactValidation.CreateAsync(contact);
                if (validation.Success)
                {
                    response = await _contactService.CreateAsync(contact);
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

        [Authorize]
        [HttpGet("ReadAsync/{idClient}")]
        public async Task<IActionResult> ReadAsync(int idClient)
        {
            ResponseModel response = new();

            try
            {
                // Valida si el numero de identificación, cumple con el formato correcto.
                ValidationModel validation = await _contactValidation.ReadOrDeleteAsync(idClient);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Conuslta el numero de identificación en BD
                    response = await _contactService.ReadAsync(idClient);

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

        [Authorize]
        [HttpPut("UpdateAsync")]
        public async Task<IActionResult> UpdateAsync([FromBody] ClientContactModel contact)
        {
            ResponseModel response = new();
            try
            {
                // Verifica si el request cumple con la estructura y valores correctos
                ValidationModel validation = await _contactValidation.UpdateAsync(contact);
                if (validation.Success)
                {
                    response = await _contactService.UpdateAsync(contact);
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

        [Authorize]
        [HttpDelete("DeleteAsync/{idClient}")]
        public async Task<IActionResult> DeleteAsync(int idClient)
        {

            ResponseModel response = new();

            try
            {
                // Valida si el numero de identificación, cumple con el formato correcto.
                ValidationModel validation = await _contactValidation.ReadOrDeleteAsync(idClient);

                // Cumple con el formato requerido
                if (validation.Success)
                {
                    // Elimina el cliente y todo lo realcionado a el en BD
                    response = await _contactService.DeleteAsync(idClient);

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
