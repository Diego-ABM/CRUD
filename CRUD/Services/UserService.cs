using CRUD.Models;
using CRUD.Models.bdCrud;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Services
{
    public class UserService
    {
        // Variables
        private readonly CrudContext _crudContext;
        private readonly InternalCode _internalCode = new();

        // Cosntructor
        public UserService(CrudContext crudContext)
        {
            _crudContext = crudContext;
        }

        // Funciones
        // Registra el usuario
        public ResponseControllerModel Create(ClientModel cliente)
        {
            ResponseControllerModel response = new();
            try
            {
                _crudContext.Cliente.Add(cliente);
                int result = _crudContext.SaveChanges();

                // Si se guarda correctamente
                if (result > 0)
                {
                    response.Code = _internalCode.Exitoso;
                    response.Message = "Cliente creado con exito";
                    response.Success = true;
                }
                else
                {
                    response.Code = _internalCode.Fallo;
                    response.Message = "No se pudo crear el cliente";
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
        public ResponseControllerModel Read(string numberIdentification)
        {
            ResponseControllerModel response = new();
            ClientModel? cliente = null;

            try
            {
                cliente = _crudContext.Cliente.Where(data => data.NumeroIdentificacion == numberIdentification).FirstOrDefault();

                if (cliente != null)
                {
                    response.Code = _internalCode.Exitoso;
                    response.Success = true;
                    response.Message = "Encontrado.";
                    response.Data = cliente;
                }
                else
                {

                    response.Code = _internalCode.Exitoso;
                    response.Success = false;
                    response.Message = "Cliente no existe.";

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrio una exepción en el proceso GetByIdentification");
                Console.WriteLine(ex.Message);
            }

            return response;
        }
        public ResponseControllerModel Update(ClientModel cliente)
        {
            ResponseControllerModel response = new();
            try
            {
                _crudContext.Cliente.Update(cliente);
                int result = _crudContext.SaveChanges();

                // Si se guarda correctamente
                if (result > 0)
                {
                    response.Code = _internalCode.Exitoso;
                    response.Message = "Cliente actualizado con exito";
                    response.Success = true;
                }
                else
                {
                    response.Code = _internalCode.Fallo;
                    response.Message = "No se pudo crear el cliente";
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
        public ResponseControllerModel Delete(string numberIdentification)
        {
            ResponseControllerModel response = new();
            try
            {
                ClientModel? client = _crudContext.Cliente.Where(c => c.NumeroIdentificacion == numberIdentification).FirstOrDefault();

                // Si no encuentra el cliente
                if (client == null)
                {
                    response.Code = _internalCode.Fallo;
                    response.Success = false;
                    response.Message = "El numero de identificación no existe";
                }
                // Cliente encontrado
                else
                {

                    _crudContext.Cliente.Remove(client);
                    int result = _crudContext.SaveChanges();

                    // Si se elimina correctamente
                    if (result > 0)
                    {
                        response.Code = _internalCode.Exitoso;
                        response.Message = "Cliente eliminado con exito";
                        response.Success = true;
                    }
                    else
                    {
                        response.Code = _internalCode.Fallo;
                        response.Message = "No se pudo eliminar el cliente";
                        response.Success = false;
                    }
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
