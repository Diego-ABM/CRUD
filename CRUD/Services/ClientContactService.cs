﻿using CRUD.Models;
using CRUD.Models.bdCrud;
using CRUD.Models.CrudBD;
using CRUD.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Services
{
    public class ClientContactService : IClientContactService
    {
        // Variables
        private readonly CrudContext _crudContext;
        private readonly InternalCode _internalCode = new();

        // Cosntructor
        public ClientContactService(CrudContext crudContext)
        {
            _crudContext = crudContext;
        }

        // Funciones
        public async Task<ResponseModel> CreateAsync(ClientContactModel contact)
        {
            ResponseModel response = new();
            try
            {
                // Prepara EF para crear la data
                _crudContext.ClienteContacto.Add(contact);

                // Intenta crear
                int result = await _crudContext.SaveChangesAsync();

                // Si se crea correctamente
                if (result > 0)
                {
                    response.Code = _internalCode.Exitoso;
                    response.Message = "Creado con exito";
                    response.Success = true;
                }
                // Fallo al crear
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
        public async Task<ResponseModel> ReadAsync(int idClient)
        {
            ResponseModel response = new();
            List<ClientContactModel> contact;

            try
            {
                // Intenta obtener la data con el parametro recibido
                contact = await _crudContext.ClienteContacto.Where(cc => cc.IdCliente == idClient).ToListAsync();

                // Si encuentra datos
                if (contact.Count != 0)
                {
                    response.Code = _internalCode.Exitoso;
                    response.Success = true;
                    response.Message = "Encontrado.";
                    response.Data = contact;
                }
                // No se encuentra datos
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
        public async Task<ResponseModel> UpdateAsync(ClientContactModel contact)
        {
            ResponseModel response = new();
            try
            {
                // Prepara EF para actualzar
                _crudContext.ClienteContacto.Update(contact);

                // Intenta actualizar
                int result = await _crudContext.SaveChangesAsync();

                // Si se guarda correctamente
                if (result > 0)
                {
                    response.Code = _internalCode.Exitoso;
                    response.Message = "Actualizado con exito.";
                    response.Success = true;
                }
                // Actualización fallo
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
        public async Task<ResponseModel> DeleteAsync(int idClient)
        {
            ResponseModel response = new();
            try
            {
                // Antes de eliminar validamos si existe en BD
                ClientContactModel? contact = _crudContext.ClienteContacto.Where(cc => cc.IdCliente == idClient).FirstOrDefault();

                // Encontrado
                if (contact != null)
                {
                    _crudContext.ClienteContacto.Remove(contact);
                    int result = await _crudContext.SaveChangesAsync();

                    // Si se elimina correctamente
                    if (result > 0)
                    {
                        response.Code = _internalCode.Exitoso;
                        response.Message = "Eliminado con exito";
                        response.Success = true;
                    }
                    // No se pudo eliminar
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
                //_logger.LogCritical($"Error in process Create {ex.Message}");
            }

            return response;
        }
    }
}
