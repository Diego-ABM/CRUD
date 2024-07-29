using CRUD.Assests;
using CRUD.Models;
using CRUD.Models.bdCrud;
using CRUD.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Services
{
    public class AuthService : IAuthService
    {
        // Variables
        private readonly CrudContext _crudContext;
        private readonly InternalCode _internalCode = new(); // Se crea por si el modelo de negocio maneja codigos internos.

        // Cosntructor
        public AuthService(CrudContext crudContext)
        {
            _crudContext = crudContext;
        }

        // Funciones
        public async Task<ResponseModel> LoginAsync(LoginModel login)
        {
            ResponseModel result = new();

            try
            {
                // Convierte la contraseña a SHA-256
                login.Contrasenia = Cifrate.PasswordToSha256(login.Contrasenia);

                // Consulta si los datos ingresados son correctos
                bool loginSuccess = await _crudContext.Usuario.AnyAsync((u) => u.CorreoElectronico == login.CorreoElectronico && u.Contrasenia == login.Contrasenia);

                // Los datos son correctos
                if (loginSuccess)
                {
                    // Seteamos la respuesta
                    result.Success = true;
                    result.Code = _internalCode.Exitoso;
                    result.Message = "Usuario logueado con exito";
                }
                // Datos incorrectos
                else
                {
                    // Seteamos la respuesta
                    result.Success = false;
                    result.Code = _internalCode.Fallo;
                    result.Message = "Usuario o contraseña incorrecto.";
                }
            }
            catch (Exception ex)
            {
                // En caso de excepcion no controlada
                result.Success = false;
                result.Code = _internalCode.Error;
                result.Message = $"Ocurrio una excepcion en el servicio LoginAsync {ex.Message}.";
            }

            return result;
        }
    }
}
