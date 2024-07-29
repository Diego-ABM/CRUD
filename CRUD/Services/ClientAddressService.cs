using CRUD.Models;
using CRUD.Models.bdCrud;
using CRUD.Models.CrudBD;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Services
{
    public class ClientAddressService : IClientAddressService
    {
        // Variables
        private readonly CrudContext _crudContext;
        private readonly InternalCode _internalCode = new();

        // Cosntructor
        public ClientAddressService(CrudContext crudContext)
        {
            _crudContext = crudContext;
        }

        // Funciones
        public async Task<ResponseModel> CreateAsync(ClientAddressModel address)
        {
            ResponseModel response = new();
            try
            {
                // Prepara a EF para agregar la data
                _crudContext.ClienteDireccion.Add(address);

                // Intenta guardar en BD
                int result = await _crudContext.SaveChangesAsync(); // Result, contiene las filas afectadas

                // Creacion exitosa
                if (result != 0)
                {
                    response.Code = _internalCode.Exitoso;
                    response.Message = "Creado con exito";
                    response.Success = true;
                }
                // No se creo
                else
                {
                    response.Code = _internalCode.Fallo;
                    response.Message = "No se pudo crear.";
                    response.Success = false;
                }

            }
            catch (DbUpdateException ex)
            {
                // En caso de excepcion al guardar en BD
                response.Code = _internalCode.Error;
                response.Message = $"Ocurrio una exepcion al guardar en BD {ex.Message}";
            }
            catch (Exception ex)
            {
                // En caso de exepcion no controlada
                response.Code = _internalCode.Error;
                response.Message = $"Ocurrio una exepcion no controlada {ex.Message}";
                //_logger.LogCritical($"Error in process Create {ex.Message}");
            }

            return response;

        }
        public async Task<ResponseModel> ReadAsync(int idClient)
        {
            ResponseModel response = new();
            List<ClientAddressModel> clientAddress;

            try
            {
                // Consulta en BD
                clientAddress = await _crudContext.ClienteDireccion.Where(cc => cc.IdCliente == idClient).ToListAsync();

                // Encontro datos
                if (clientAddress.Count != 0)
                {
                    response.Code = _internalCode.Exitoso;
                    response.Success = true;
                    response.Message = "Encontrado.";
                    response.Data = clientAddress;
                }
                // No entontro datos
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
        public async Task<ResponseModel> UpdateAsync(ClientAddressModel clientAddress)
        {
            ResponseModel response = new();
            try
            {
                // Prepara EF para actualzar la data
                _crudContext.ClienteDireccion.Update(clientAddress);

                // Intenta actualizar
                int result = await _crudContext.SaveChangesAsync();

                // Si se actualiza correctamente
                if (result > 0)
                {
                    response.Code = _internalCode.Exitoso;
                    response.Message = "Actualizado con exito.";
                    response.Success = true;
                }
                // Fallo la actualización
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
            }

            return response;
        }
        public async Task<ResponseModel> DeleteAsync(int id)
        {
            ResponseModel response = new();
            try
            {
                // Antes de eliminar validamos si existe en BD
                ClientAddressModel? clientEmail = await _crudContext.ClienteDireccion.Where(cc => cc.Id == id).FirstOrDefaultAsync();

                // Encontrado
                if (clientEmail != null)
                {
                    // Preparamos EF para eliminar
                    _crudContext.ClienteDireccion.Remove(clientEmail);

                    // Intenta eliminar
                    int result = await _crudContext.SaveChangesAsync();

                    // Si se elimina correctamente
                    if (result > 0)
                    {
                        response.Code = _internalCode.Exitoso;
                        response.Message = "Eliminado con exito";
                        response.Success = true;
                    }
                    // Fallo la elimiancion
                    else
                    {
                        response.Code = _internalCode.Fallo;
                        response.Message = "No se pudo eliminar.";
                        response.Success = false;
                    }

                }
                // No encontrado, no es necesario continuar
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
