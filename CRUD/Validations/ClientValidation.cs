using CRUD.Models;
using CRUD.Models.bdCrud;
using CRUD.Models.CrudBD;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;

namespace CRUD.Validations
{
    public class ClientValidation
    {
        // Variables
        private readonly IdentificationTypeModel _identificationTypeStruct = new();
        private readonly InternalCode _internalCodes = new();

        // Funciones
        public async Task<ValidationModel> CreateAsync(ClientModel client)
        {
            ValidationModel validation = new();
            ConcurrentDictionary<string, List<string>> erros = [];

            try
            {
                // Crear una lista de tareas
                List<Task> tasks =
                [
                    Task.Run(() => ValidateId(erros, client.IdUsuario)),
                    Task.Run(() => ValidateName(erros, client.Nombre)),
                    Task.Run(() => ValidateAge(erros, client.Edad)),
                    Task.Run(() => ValidateIdentificationType(erros, client.IdTipoIdentificacion)),
                    Task.Run(() => ValidateIdentification(erros, client.NumeroIdentificacion))
                ];

                // Esperar a que todas las tareas se completen
                await Task.WhenAll(tasks);

                validation.Erros = erros.ToDictionary();

                if (validation.Erros.IsNullOrEmpty())
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
        public async Task<ValidationModel> ReadOrDeleteAsync(string identificationNumber)
        {

            ValidationModel validation = new();
            ConcurrentDictionary<string, List<string>> erros = [];

            try
            {
                await Task.Run(() => ValidateIdentification(erros, identificationNumber));

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
            return validation;
        }
        public async Task<ValidationModel> UpdateAsync(ClientModel client)
        {
            ValidationModel validation = new();
            ConcurrentDictionary<string, List<string>> erros = [];

            try
            {
                // Crear una lista de tareas
                List<Task> tasks =
                [

                    Task.Run(() => ValidateId(erros, client.Id)),
                    Task.Run(() => ValidateIdClient(erros, client.IdUsuario)),
                    Task.Run(() => ValidateName(erros, client.Nombre)),
                    Task.Run(() => ValidateAge(erros, client.Edad)),
                    Task.Run(() => ValidateIdentificationType(erros, client.IdTipoIdentificacion)),
                    Task.Run(() => ValidateIdentification(erros, client.NumeroIdentificacion))
                ];

                // Esperar a que todas las tareas se completen
                await Task.WhenAll(tasks);

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
        private static void ValidateId(ConcurrentDictionary<string, List<string>> erros, int id)
        {
            if (id == 0)
            {
                erros.TryAdd("id", ["La clave id es requerida, su valor no puede ser 0"]);
            }
        }
        private static void ValidateIdClient(ConcurrentDictionary<string, List<string>> erros, int idClient)
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
                erros.TryAdd("nombre", ["No puede estar vacio"]);
            }
            else if (name.Any(char.IsDigit))
            {
                erros.TryAdd("nombre", ["No se aceptan numeros."]);
            }
            else if (name.Length > 100)
            {
                erros.TryAdd("nombre", ["Numero Maximo de caracteres aceptados 100."]);
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
            // Valida si el tipo de identificación es valido
            bool isOk = _identificationTypeStruct.IdentificationTypes.ContainsKey(IdTipoIdentificacion);

            if (!isOk)
            {
                // Agrega la entrada al diccionario de errores
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
    }
}
