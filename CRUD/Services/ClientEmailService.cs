﻿using CRUD.Models;
using CRUD.Models.bdCrud;
using CRUD.Models.CrudBD;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Services
{
    public class ClientEmailService
    {
        // Variables
        private readonly CrudContext _crudContext;
        private readonly InternalCode _internalCode = new();

        // Cosntructor
        public ClientEmailService(CrudContext crudContext)
        {
            _crudContext = crudContext;
        }

        // Funciones
        public ResponseModel Create(ClientEmailModel email)
        {
            ResponseModel response = new();
            try
            {
                _crudContext.ClienteCorreoElectronico.Add(email);
                int result = _crudContext.SaveChanges();

                // Si se guarda correctamente
                if (result != 0)
                {
                    response.Code = _internalCode.Exitoso;
                    response.Message = "Creado con exito";
                    response.Success = true;
                }
                else
                {
                    response.Code = _internalCode.Fallo;
                    response.Message = "No se pudo crear.";
                    response.Success = false;
                }

            }
            catch (DbUpdateException ex)
            {
                response.Code = _internalCode.Error;
                response.Message = $"Ocurrio una exepcion al guardar en BD {ex.Message}";
            }
            catch (Exception ex)
            {
                response.Code = _internalCode.Error;
                response.Message = $"Ocurrio una exepcion no controlada {ex.Message}";
                //_logger.LogCritical($"Error in process Create {ex.Message}");
            }

            return response;

        }
        public ResponseModel Read(int idClient, string email = "")
        {
            ResponseModel response = new();
            List<ClientEmailModel> contactEmail;

            try
            {
                // Valida si el parametro opcional fue diligenciado
                if (string.IsNullOrEmpty(email))
                {
                    // Busca solo por ID y regresa los correos relacionados a ese ID
                    contactEmail = _crudContext.ClienteCorreoElectronico.Where(cc => cc.IdCliente == idClient).ToList();
                }
                else
                {
                    // Busca por ID y correo
                    contactEmail = _crudContext.ClienteCorreoElectronico.Where(cc => cc.IdCliente == idClient && cc.CorreoElectronico == email).ToList();
                }

                if (contactEmail.Count != 0)
                {
                    response.Code = _internalCode.Exitoso;
                    response.Success = true;
                    response.Message = "Encontrado.";
                    response.Data = contactEmail;
                }
                else
                {
                    response.Code = _internalCode.Fallo;
                    response.Success = false;
                    response.Message = "No existe.";

                }

            }
            catch (Exception ex)
            {
                response.Code = _internalCode.Error;
                response.Success = false;
                response.Message = $"Ocurrio una exepción no controlada {ex.Message}";
            }

            return response;
        }
        public ResponseModel Update(ClientEmailModel email)
        {
            ResponseModel response = new();
            try
            {
                _crudContext.ClienteCorreoElectronico.Update(email);

                int result = _crudContext.SaveChanges();

                // Si se guarda correctamente
                if (result > 0)
                {
                    response.Code = _internalCode.Exitoso;
                    response.Message = "Actualizado con exito.";
                    response.Success = true;
                }
                else
                {
                    response.Code = _internalCode.Fallo;
                    response.Message = "No se pudo actualizar.";
                    response.Success = false;
                }

            }
            catch (DbUpdateException ex)
            {
                response.Code = _internalCode.Error;
                response.Message = $"Ocurrio una exepcion al actualizar en BD {ex.Message}";
            }
            catch (Exception ex)
            {
                response.Code = _internalCode.Error;
                response.Message = $"Ocurrio una exepcion no controlada {ex.Message}";
                //_logger.LogCritical($"Error in process Create {ex.Message}");
            }

            return response;
        }
        public ResponseModel Delete(int id)
        {
            ResponseModel response = new();
            try
            {
                // Antes de eliminar validamos si existe en BD
                ClientEmailModel? clientEmail = _crudContext.ClienteCorreoElectronico
                    .Where(cc => cc.Id == id).FirstOrDefault();

                // Encontrado
                if (clientEmail != null)
                {
                    _crudContext.ClienteCorreoElectronico.Remove(clientEmail);
                    int result = _crudContext.SaveChanges();

                    // Si se elimina correctamente
                    if (result > 0)
                    {
                        response.Code = _internalCode.Exitoso;
                        response.Message = "Eliminado con exito";
                        response.Success = true;
                    }
                    else
                    {
                        response.Code = _internalCode.Fallo;
                        response.Message = "No se pudo eliminar.";
                        response.Success = false;
                    }

                }
                // No encontrado
                else
                {
                    response.Code = _internalCode.Fallo;
                    response.Success = false;
                    response.Message = "No existe";
                }
            }
            catch (DbUpdateException ex)
            {
                response.Code = _internalCode.Error;
                response.Message = $"Ocurrio una exepcion al guardar en BD {ex.Message}";
            }
            catch (Exception ex)
            {
                response.Code = _internalCode.Error;
                response.Message = $"Ocurrio una exepcion no controlada {ex.Message}";
            }

            return response;
        }
    }
}
