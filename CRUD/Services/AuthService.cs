using CRUD.Assests;
using CRUD.Models;
using CRUD.Models.bdCrud;

namespace CRUD.Services
{
    public class AuthService
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
        public ResponseControllerModel Login(LoginModel login)
        {
            ResponseControllerModel result = new();

            try
            {
                login.Contrasenia = Cifrate.PasswordToSha256(login.Contrasenia);

                bool loginSuccess = _crudContext.Usuario.Any((u) => u.CorreoElectronico == login.CorreoElectronico && u.Contrasenia == login.Contrasenia);

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
                result.Message = $"Ocurrio una excepcion en el servicio Login {ex.Message}.";
            }

            return result;
        }

    }
}
