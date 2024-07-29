using CRUD.Models;
using CRUD.Models.bdCrud;
using CRUD.Models.CrudBD;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Services
{
    public class ClientEmailService : IClientEmailService
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
        public async Task<ResponseModel> CreateAsync(ClientEmailModel email)
        {
            ResponseModel response = new();
            try
            {
                // Prepara EF para crear
                _crudContext.ClienteCorreoElectronico.Add(email);

                // Intenta crear
                int result = await _crudContext.SaveChangesAsync();

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
        public async Task<ResponseModel> ReadAsync(int idClient, string email = "")
        {
            ResponseModel response = new();
            List<ClientEmailModel> contactEmail;

            try
            {
                // Valida si el parametro opcional fue diligenciado
                if (string.IsNullOrEmpty(email))
                {
                    // Busca solo por ID y regresa los correos relacionados a ese ID
                    contactEmail = await _crudContext.ClienteCorreoElectronico.Where(cc => cc.IdCliente == idClient).ToListAsync();
                }
                else
                {
                    // Busca por ID y correo
                    contactEmail = await _crudContext.ClienteCorreoElectronico.Where(cc => cc.IdCliente == idClient && cc.CorreoElectronico == email).ToListAsync();
                }

                // Si encuntra datos
                if (contactEmail.Count != 0)
                {
                    response.Code = _internalCode.Exitoso;
                    response.Success = true;
                    response.Message = "Encontrado.";
                    response.Data = contactEmail;
                }
                // No encuentra datos
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
        public async Task<ResponseModel> UpdateAsync(ClientEmailModel email)
        {
            ResponseModel response = new();
            try
            {
                // Prepara EF para actualizar
                _crudContext.ClienteCorreoElectronico.Update(email);

                // Intenta actualizar
                int result = await _crudContext.SaveChangesAsync();

                // Si se guarda correctamente
                if (result > 0)
                {
                    response.Code = _internalCode.Exitoso;
                    response.Message = "Actualizado con exito.";
                    response.Success = true;
                }
                // Fallo al guardar
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
        public async Task<ResponseModel> DeleteAsync(int id)
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
                    // Prepara EF para eliminar
                    _crudContext.ClienteCorreoElectronico.Remove(clientEmail);

                    // Intenta eliminar
                    int result = await _crudContext.SaveChangesAsync();

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
