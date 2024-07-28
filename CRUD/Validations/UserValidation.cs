using CRUD.Models;
using CRUD.Models.CrudBD;
using CRUD.Models.CrudBD.Structs;
using System.Text.RegularExpressions;

namespace CRUD.Validations
{
    public class UserValidation
    {
        // Variables
        private readonly IdentificationTypeStruct _identificationTypeStruct = new();
        private readonly InternalCode _internalCodes = new();

        // Funciones
        public ValidationModel Create(UserModel user)
        {
            ValidationModel validation = new();
            Dictionary<string, List<string>> erros = [];

            try
            {
                ValidateName(erros, user.Nombre);
                ValidateUserName(erros, user.Usuario);
                ValidatePassword(erros, user.Contrasenia);
                ValidateEmail(erros, user.CorreoElectronico);
                ValidateAge(erros, user.Edad);
                ValidateIdentificationType(erros, user.IdTipoIdentificacion);
                ValidateIdentification(erros, user.NumeroIdentificacion);

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
                    validation.Message = "Request cliente contiene errores";
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
        public ValidationModel ReadOrDelete(string email)
        {

            ValidationModel validation = new();
            Dictionary<string, List<string>> erros = [];

            try
            {
                ValidateEmail(erros, email);

                validation.Erros = erros;

                if (validation.Erros.Count == 0)
                {
                    validation.Code = _internalCodes.Exitoso;
                    validation.Success = true;
                    validation.Message = "Validacion exitosa";
                }
                else
                {
                    validation.Code = _internalCodes.Fallo;
                    validation.Success = false;
                    validation.Message = "Request usuario contiene errores";
                }

            }
            catch (Exception ex)
            {
                validation.Code = _internalCodes.Error;
                validation.Success = false;
                validation.Message = ex.Message;
            }
            return validation;
        }
        public ValidationModel Update(UserModel user)
        {
            ValidationModel validation = new();
            Dictionary<string, List<string>> erros = [];

            try
            {
                ValidateId(erros, user.Id);
                ValidateName(erros, user.Nombre);
                ValidateUserName(erros, user.Usuario);
                ValidatePassword(erros, user.Contrasenia);
                ValidateEmail(erros, user.CorreoElectronico);
                ValidateAge(erros, user.Edad);
                ValidateIdentificationType(erros, user.IdTipoIdentificacion);
                ValidateIdentification(erros, user.NumeroIdentificacion);

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
                    validation.Message = "Request cliente contiene errores";
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
        private Dictionary<string, List<string>> ValidateId(Dictionary<string, List<string>> erros, int idClient)
        {
            if (idClient == 0)
            {
                erros.Add("id", ["La clave id es requerida, su valor no puede ser 0"]);
            }

            return erros;
        }
        private Dictionary<string, List<string>> ValidateName(Dictionary<string, List<string>> erros, string name)
        {
            // Any(char.IsDigit) valida que el nombre no tenga numeros
            if (string.IsNullOrEmpty(name))
            {
                erros.Add("nombre", ["Campo requerido."]);
            }
            else if (name.Any(char.IsDigit))
            {
                erros.Add("nombre", ["No debe contener numeros."]);
            }
            else if (name.Length > 100)
            {
                erros.Add("nombre", ["Numero Maximo de caracteres aceptados 100."]);
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
        private Dictionary<string, List<string>> ValidateUserName(Dictionary<string, List<string>> erros, string userName)
        {
            // Expresion regular para validar que no tenga espacios en blanco
            string pattern = @"^\S+$";

            if (string.IsNullOrEmpty(userName))
            {
                erros.Add("usuario", ["El valor no puede estar vacio."]);
            }
            else if (!Regex.IsMatch(userName, pattern))
            {
                erros.Add("usuario", ["No puede contener espacios en blanco."]);
            }
            else if (userName.Length > 100)
            {
                erros.Add("usario", ["Numero Maximo de caracteres aceptados 100."]);
            }
            return erros;
        }
        private Dictionary<string, List<string>> ValidateAge(Dictionary<string, List<string>> erros, byte age)
        {

            if (age == 0)
            {
                erros.Add("edad", ["La clave edad es requerida, su valor no puede ser 0"]);
            }
            else if (age < 18)
            {
                erros.Add("edad", ["El cliente debe ser mayor de 18"]);
            }

            return erros;
        }
        private Dictionary<string, List<string>> ValidateIdentificationType(Dictionary<string, List<string>> erros, int IdTipoIdentificacion)
        {
            // Valida si el tipo de identificaion es el esperado
            bool isOk = _identificationTypeStruct.IdentificationType.ContainsKey(IdTipoIdentificacion);

            if (!isOk)
            {
                // Agrega la entrada al diccionario
                erros.Add("idTipoIdentificacion",
                    // El Select, recorre la lista y muestra todos los posibles valores
                    _identificationTypeStruct.IdentificationType
                    .Select(kv => $"Posible valor : {kv.Key}; Significado : {kv.Value}")
                    .ToList());
            }

            return erros;
        }
        private Dictionary<string, List<string>> ValidateIdentification(Dictionary<string, List<string>> erros, string identification)
        {
            if (string.IsNullOrEmpty(identification))
            {
                erros.Add("numeroIdentificacion", ["El numero de identifiacion es requerido."]);
            }
            else if (identification.Length > 20)
            {
                erros.Add("numeroIdentificacion", ["Numero Maximo de caracteres aceptados 20."]);
            }

            return erros;

        }
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
    }
}
