using CRUD.Models;
using System.Text.RegularExpressions;

namespace CRUD.Validations
{
    public class AuthValidation
    {
        // Variables
        private readonly InternalCode _internalCodes = new();

        // Funciones
        public ValidationModel Login(LoginModel login)
        {
            ValidationModel validation = new();
            Dictionary<string, List<string>> erros = [];

            try
            {
                ValidateEmail(erros, login.CorreoElectronico);
                ValidatePassword(erros, login.Contrasenia);

                validation.Erros = erros;

                if (validation.Erros.Count == 0)
                {
                    validation.Code = _internalCodes.Exitoso;
                    validation.Success = true;
                    validation.Message = "Validacion exitosa";
                }
                else
                {
                    validation.Code = _internalCodes.Exitoso;
                    validation.Success = false;
                    validation.Message = "Request Login contiene errores";
                }

            }
            catch (Exception ex)
            {
                validation.Code = _internalCodes.Error;
                validation.Success = false;
                validation.Message = ex.Message;
            }

            // Valida si el tipo de documento ingresado es el esperado
            return validation;
        }


        // Validaciones
        private Dictionary<string, List<string>> ValidateEmail(Dictionary<string, List<string>> erros, string email)
        {
            // Expresión regular para validar que el strign sea un correo electrónico
            string pattern = @"^[\w.-]+@[a-zA-Z\d.-]+\.[a-zA-Z]{2,}$";

            if (string.IsNullOrEmpty(email))
            {
                erros.Add("correoElectronico", ["Correo electronico es requerido."]);
            }
            else if (!Regex.IsMatch(email, pattern))
            {
                erros.Add("correoElectronico", ["El formato no es valido."]);
            }
            else if (email.Length > 20)
            {
                erros.Add("correoElectronico", ["Numero Maximo de caracteres aceptados 100."]);
            }

            return erros;

        }
        private Dictionary<string, List<string>> ValidatePassword(Dictionary<string, List<string>> erros, string password)
        {
            // Expresion regular para contraseñas generales segun ISO/IEC 27002:2013
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&. ])[\w\d@$!%*?&. ]{8,}$";

            if (string.IsNullOrEmpty(password))
            {
                erros.Add("contrasenia", ["Campo requerido."]);
            }
            else if (!Regex.IsMatch(password, pattern))
            {
                erros.Add("contrasenia", ["La contraseña debe tener como mínimo 8 caracteres, incluyendo al menos una letra mayúscula, una letra minúscula, un número y un carácter especial."]);
            }
            else if (password.Length > 100)
            {
                erros.Add("nombre", ["Numero Maximo de caracteres aceptados 100."]);
            }
            return erros;
        }
    }
}
