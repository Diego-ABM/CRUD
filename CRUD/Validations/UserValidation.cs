using CRUD.Models;
using CRUD.Models.CrudBD;
using CRUD.Validations.Interfaces;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace CRUD.Validations
{
    public class UserValidation : IUserValidation
    {
        // Variables
        private readonly IdentificationTypeModel _identificationTypeStruct = new();
        private readonly InternalCode _internalCodes = new();

        // Funciones
        public async Task<ValidationModel> CreateAsync(UserModel user)
        {
            ValidationModel validation = new();
            ConcurrentDictionary<string, List<string>> erros = [];

            try
            {
                List<Task> tareas =
                [
                    Task.Run(() => ValidateName(erros, user.Nombre)),
                    Task.Run(() => ValidateUserName(erros, user.Usuario)),
                    Task.Run(() => ValidatePassword(erros, user.Contrasenia)),
                    Task.Run(() => ValidateEmail(erros, user.CorreoElectronico)),
                    Task.Run(() => ValidateAge(erros, user.Edad)),
                    Task.Run(() => ValidateIdentificationType(erros, user.IdTipoIdentificacion)),
                    Task.Run(() => ValidateIdentification(erros, user.NumeroIdentificacion))
                ];

                // Esperar a que todas las tareas se completen
                await Task.WhenAll(tareas);

                validation.Erros = erros.ToDictionary();

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
        public async Task<ValidationModel> ReadOrDeleteAsync(string email)
        {

            ValidationModel validation = new();
            ConcurrentDictionary<string, List<string>> erros = [];

            try
            {
                await Task.Run(() => ValidateEmail(erros, email));

                validation.Erros = erros.ToDictionary();

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
        public async Task<ValidationModel> UpdateAsync(UserModel user)
        {
            ValidationModel validation = new();
            ConcurrentDictionary<string, List<string>> erros = [];

            try
            {

                List<Task> tareas =
                [
                    Task.Run(() => ValidateId(erros, user.Id)),
                    Task.Run(() => ValidateName(erros, user.Nombre)),
                    Task.Run(() => ValidateUserName(erros, user.Usuario)),
                    Task.Run(() => ValidatePassword(erros, user.Contrasenia)),
                    Task.Run(() => ValidateEmail(erros, user.CorreoElectronico)),
                    Task.Run(() => ValidateAge(erros, user.Edad)),
                    Task.Run(() => ValidateIdentificationType(erros, user.IdTipoIdentificacion)),
                    Task.Run(() => ValidateIdentification(erros, user.NumeroIdentificacion))
                ];

                // Esperar a que todas las tareas se completen
                await Task.WhenAll(tareas);

                validation.Erros = erros.ToDictionary();

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
        private static void ValidateId(ConcurrentDictionary<string, List<string>> erros, int idClient)
        {
            if (idClient == 0)
            {
                erros.TryAdd("id", ["La clave id es requerida, su valor no puede ser 0"]);
            }

        }
        private static void ValidateName(ConcurrentDictionary<string, List<string>> erros, string name)
        {
            // Any(char.IsDigit) valida que el nombre no tenga numeros
            if (string.IsNullOrEmpty(name))
            {
                erros.TryAdd("nombre", ["Campo requerido."]);
            }
            else if (name.Any(char.IsDigit))
            {
                erros.TryAdd("nombre", ["No debe contener numeros."]);
            }
            else if (name.Length > 100)
            {
                erros.TryAdd("nombre", ["Numero Maximo de caracteres aceptados 100."]);
            }

        }
        private static void ValidatePassword(ConcurrentDictionary<string, List<string>> erros, string password)
        {
            // Expresion regular para contraseñas generales segun ISO/IEC 27002:2013
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&. ])[\w\d@$!%*?&. ]{8,}$";

            if (string.IsNullOrEmpty(password))
            {
                erros.TryAdd("contrasenia", ["Campo requerido."]);
            }
            else if (!Regex.IsMatch(password, pattern))
            {
                erros.TryAdd("contrasenia", ["La contraseña debe tener como mínimo 8 caracteres, incluyendo al menos una letra mayúscula, una letra minúscula, un número y un carácter especial."]);
            }
            else if (password.Length > 100)
            {
                erros.TryAdd("nombre", ["Numero Maximo de caracteres aceptados 100."]);
            }

        }
        private static void ValidateUserName(ConcurrentDictionary<string, List<string>> erros, string userName)
        {
            // Expresion regular para validar que no tenga espacios en blanco
            string pattern = @"^\S+$";

            if (string.IsNullOrEmpty(userName))
            {
                erros.TryAdd("usuario", ["El valor no puede estar vacio."]);
            }
            else if (!Regex.IsMatch(userName, pattern))
            {
                erros.TryAdd("usuario", ["No puede contener espacios en blanco."]);
            }
            else if (userName.Length > 100)
            {
                erros.TryAdd("usario", ["Numero Maximo de caracteres aceptados 100."]);
            }

        }
        private static void ValidateAge(ConcurrentDictionary<string, List<string>> erros, byte age)
        {

            if (age == 0)
            {
                erros.TryAdd("edad", ["La clave edad es requerida, su valor no puede ser 0"]);
            }
            else if (age < 18)
            {
                erros.TryAdd("edad", ["El cliente debe ser mayor de 18"]);
            }


        }
        private void ValidateIdentificationType(ConcurrentDictionary<string, List<string>> erros, int IdTipoIdentificacion)
        {
            // Valida si el tipo de identificaion es el esperado
            bool isOk = _identificationTypeStruct.IdentificationTypes.ContainsKey(IdTipoIdentificacion);

            if (!isOk)
            {
                // Agrega la entrada al diccionario
                erros.TryAdd("idTipoIdentificacion",
                    // El Select, recorre la lista y muestra todos los posibles valores
                    _identificationTypeStruct.IdentificationTypes
                    .Select(kv => $"Posible valor : {kv.Key}; Significado : {kv.Value}")
                    .ToList());
            }


        }
        private static void ValidateIdentification(ConcurrentDictionary<string, List<string>> erros, string identification)
        {
            if (string.IsNullOrEmpty(identification))
            {
                erros.TryAdd("numeroIdentificacion", ["El numero de identifiacion es requerido."]);
            }
            else if (identification.Length > 20)
            {
                erros.TryAdd("numeroIdentificacion", ["Numero Maximo de caracteres aceptados 20."]);
            }



        }
        private static void ValidateEmail(ConcurrentDictionary<string, List<string>> erros, string email)
        {
            // Expresión regular para validar que el strign sea un correo electrónico
            string pattern = @"^[\w.-]+@[a-zA-Z\d.-]+\.[a-zA-Z]{2,}$";

            if (string.IsNullOrEmpty(email))
            {
                erros.TryAdd("correoElectronico", ["Correo electronico es requerido."]);
            }
            else if (!Regex.IsMatch(email, pattern))
            {
                erros.TryAdd("correoElectronico", ["El formato no es valido."]);
            }
            else if (email.Length > 100)
            {
                erros.TryAdd("correoElectronico", ["Numero Maximo de caracteres aceptados 100."]);
            }



        }
    }
}
