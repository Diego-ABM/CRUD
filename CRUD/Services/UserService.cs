using CRUD.Assests;
using CRUD.Models;
using CRUD.Models.bdCrud;
using CRUD.Models.CrudBD;
using Microsoft.EntityFrameworkCore;


namespace CRUD.Services
{
    public class UserService : IUserService
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
        public async Task<ResponseModel> CreateAsync(UserModel user)
        {
            ResponseModel response = new();
            try
            {
                // Validamos si el correo no se ecuentra registrado, ya que es tipo UNIQUE
                int result = _crudContext.Usuario.Where(x => x.CorreoElectronico == user.CorreoElectronico).Select(x => x.Id).FirstOrDefault();

                // Si el usuario no existe
                if (result == 0)
                {
                    // Cifra la contraseña con el algoritmos SHA-256
                    user.Contrasenia = Cifrate.PasswordToSha256(user.Contrasenia);

                    _crudContext.Usuario.Add(user);
                    result = await _crudContext.SaveChangesAsync();

                    // Si se guarda correctamente
                    if (result > 0)
                    {
                        response.Code = _internalCode.Exitoso;
                        response.Message = "Usuario creado con exito";
                        response.Success = true;
                    }
                    else
                    {
                        response.Code = _internalCode.Fallo;
                        response.Message = "No se pudo crear el usuario";
                        response.Success = false;
                    }
                }
                else
                {
                    response.Code = _internalCode.Fallo;
                    response.Success = false;
                    response.Message = $"El correo electronico se encuentra registrado {user.CorreoElectronico}";
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
        public async Task<ResponseModel> ReadAsync(string email)
        {
            ResponseModel response = new();
            UserModel? usuario = null;

            try
            {
                usuario = await _crudContext.Usuario.Where(data => data.CorreoElectronico == email).FirstOrDefaultAsync();

                if (usuario != null)
                {
                    response.Code = _internalCode.Exitoso;
                    response.Success = true;
                    response.Message = "Encontrado.";
                    response.Data = usuario;
                }
                else
                {
                    response.Code = _internalCode.Fallo;
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
        public async Task<ResponseModel> UpdateAsync(UserModel user)
        {
            ResponseModel response = new();
            try
            {
                // Validamos si el correo no se ecuentra registrado, ya que es tipo UNIQUE
                int result = await _crudContext.Usuario.Where(x => x.CorreoElectronico == user.CorreoElectronico).Select(x => x.Id).FirstOrDefaultAsync();

                // Si el usuario no existe
                if (result == 0)
                {
                    // Cifra la contraseña con el algoritmos SHA-256
                    user.Contrasenia = Cifrate.PasswordToSha256(user.Contrasenia);

                    _crudContext.Usuario.Update(user);
                    result = await _crudContext.SaveChangesAsync();

                    // Si se guarda correctamente
                    if (result > 0)
                    {
                        response.Code = _internalCode.Exitoso;
                        response.Message = "Usuario actualizado con exito";
                        response.Success = true;
                    }
                    else
                    {
                        response.Code = _internalCode.Fallo;
                        response.Message = "No se pudo actualizar el usuario";
                        response.Success = false;
                    }
                }
                else
                {
                    response.Code = _internalCode.Fallo;
                    response.Success = false;
                    response.Message = $"El correo electronico se encuentra en uso {user.CorreoElectronico}";
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
        public async Task<ResponseModel> DeleteAsync(string email)
        {
            ResponseModel response = new();
            try
            {
                UserModel? usuer = await _crudContext.Usuario.Where(c => c.CorreoElectronico == email).FirstOrDefaultAsync();

                // Si no encuentra el usuario
                if (usuer == null)
                {
                    response.Code = _internalCode.Fallo;
                    response.Success = false;
                    response.Message = $"El correo {email} no existe";
                }
                // Usuario encontrado
                else
                {
                    _crudContext.Usuario.Remove(usuer);
                    int result = await _crudContext.SaveChangesAsync();

                    // Si se elimina correctamente
                    if (result > 0)
                    {
                        response.Code = _internalCode.Exitoso;
                        response.Message = "Usuario eliminado con exito";
                        response.Success = true;
                    }
                    else
                    {
                        response.Code = _internalCode.Fallo;
                        response.Message = "No se pudo eliminar el usuario";
                        response.Success = false;
                    }
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
    }
}
