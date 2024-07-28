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
        private readonly InternalCode _internalCode = new();

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
                login.Contrasenia = Cifrate.PasswordToSha256(login.Contrasenia);

                bool loginSuccess = await _crudContext.Usuario.AnyAsync((u) => u.CorreoElectronico == login.CorreoElectronico && u.Contrasenia == login.Contrasenia);

                if (loginSuccess)
                {
                    result.Success = true;
                    result.Code = _internalCode.Exitoso;
                    result.Message = "Usuario logueado con exito";
                }
                else
                {
                    result.Success = false;
                    result.Code = _internalCode.Fallo;
                    result.Message = "Usuario o contraseña incorrecto.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Code = _internalCode.Error;
                result.Message = $"Ocurrio una excepcion en el servicio LoginAsync {ex.Message}.";
            }

            return result;
        }

    }
}
