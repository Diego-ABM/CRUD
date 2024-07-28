using CRUD.Assests;
using CRUD.Models;
using CRUD.Models.bdCrud;
using CRUD.Models.CrudBD;
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
        public ResponseModel Create(UserModel user)
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
                    result = _crudContext.SaveChanges();

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
        // Consulta el usuario por correo
        public ResponseModel Read(string email)
        {
            ResponseModel response = new();
            UserModel? usuario = null;

            try
            {
                usuario = _crudContext.Usuario.Where(data => data.CorreoElectronico == email).FirstOrDefault();

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
        public ResponseModel Update(UserModel user)
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

                    _crudContext.Usuario.Update(user);
                    result = _crudContext.SaveChanges();

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
        public ResponseModel Delete(string email)
        {
            ResponseModel response = new();
            try
            {
                UserModel? usuer = _crudContext.Usuario.Where(c => c.CorreoElectronico == email).FirstOrDefault();

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
                    int result = _crudContext.SaveChanges();

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
