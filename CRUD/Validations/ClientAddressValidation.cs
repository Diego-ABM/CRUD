using CRUD.Models;
using CRUD.Models.CrudBD;
using CRUD.Validations.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;

namespace CRUD.Validations
{
    public class ClientAddressValidation : IClientAddressValidation
    {
        // Variables
        private readonly CountryModel _countryModel = new();
        private readonly InternalCode _internalCodes = new();

        // Funciones
        public async Task<ValidationModel> CreateAsync(ClientAddressModel clientAddress)
        {
            ValidationModel validation = new();
            //Maneja la concurrencia
            ConcurrentDictionary<string, List<string>> erros = [];

            try
            {
                // Crear una lista de tareas
                List<Task> tareas =
                    [
                        Task.Run(() => ValidateIdClient(erros, clientAddress.IdCliente)),
                        Task.Run(() => ValidateAddress(erros, clientAddress.Direccion)),
                        Task.Run(() => ValidateCity(erros, clientAddress.Ciudad)),
                        Task.Run(() => ValidateCodePostal(erros, clientAddress.CodigoPostal)),
                        Task.Run(() => ValidateIdcountry(erros, clientAddress.IdCodigoPais))
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
                    validation.Code = _internalCodes.Fallo;
                    validation.Success = false;
                    validation.Message = "Request contiene errores";
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
        public async Task<ValidationModel> ReadOrDeleteAsync(int id)
        {

            ValidationModel validation = new();
            ConcurrentDictionary<string, List<string>> erros = [];

            try
            {
                await Task.Run(() => ValidateId(erros, id));

                validation.Erros = erros.ToDictionary();

                if (validation.Erros.IsNullOrEmpty())
                {
                    validation.Code = _internalCodes.Exitoso;
                    validation.Success = true;
                    validation.Message = "Validacion exitosa";
                }
                else
                {
                    validation.Code = _internalCodes.Fallo;
                    validation.Success = false;
                    validation.Message = "Request contiene errores";
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
        public async Task<ValidationModel> UpdateAsync(ClientAddressModel clientAddress)
        {
            ValidationModel validation = new();
            ConcurrentDictionary<string, List<string>> erros = [];

            try
            {
                // Crear una lista de tareas
                List<Task> tareas =
                    [
                        Task.Run(() => ValidateId(erros, clientAddress.Id)),
                        Task.Run(() => ValidateIdClient(erros, clientAddress.IdCliente)),
                        Task.Run(() => ValidateAddress(erros, clientAddress.Direccion)),
                        Task.Run(() => ValidateCity(erros, clientAddress.Ciudad)),
                        Task.Run(() => ValidateCodePostal(erros, clientAddress.CodigoPostal)),
                        Task.Run(() => ValidateIdcountry(erros, clientAddress.IdCodigoPais))
                    ];

                // Esperar a que todas las tareas se completen
                await Task.WhenAll(tareas);

                validation.Erros = erros.ToDictionary();

                if (validation.Erros.IsNullOrEmpty())
                {
                    validation.Code = _internalCodes.Exitoso;
                    validation.Success = true;
                    validation.Message = "Validacion exitosa";
                }
                else
                {
                    validation.Code = _internalCodes.Fallo;
                    validation.Success = false;
                    validation.Message = "Request contiene errores";
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
            if (id == 0) erros.TryAdd("id", ["El id es requerido, su valor no puede ser 0"]);
        }
        private static void ValidateIdClient(ConcurrentDictionary<string, List<string>> erros, int idClient)
        {
            if (idClient == 0) erros.TryAdd("idCliente", ["El id es requerido, su valor no puede ser 0"]);
        }
        private static void ValidateAddress(ConcurrentDictionary<string, List<string>> erros, string address)
        {

            if (string.IsNullOrEmpty(address))
            {
                // Agrega la entrada al diccionario
                erros.TryAdd("direccion", ["No puede estar vacio"]);
            }
            if (address.Length > 255)
            {
                erros.TryAdd("direccion", ["Suepera la cantidad maxima de caracteres permitidos 255"]);
            }
        }
        private static void ValidateCity(ConcurrentDictionary<string, List<string>> erros, string city)
        {

            if (string.IsNullOrEmpty(city))
            {
                erros.TryAdd("ciudad", ["No puede estar vacio"]);
            }
            if (city.Length > 100)
            {
                erros.TryAdd("ciudad", ["Supera la cantidad maxima de caracteres permitidos 100"]);
            }
        }
        private static void ValidateCodePostal(ConcurrentDictionary<string, List<string>> erros, string codePostal)
        {

            if (string.IsNullOrEmpty(codePostal))
            {
                // Agrega la entrada al diccionario
                erros.TryAdd("ciudad", ["No puede estar vacio"]);
            }
            if (codePostal.Length > 10)
            {
                erros.TryAdd("ciudad", ["Supera la cantidad maxima de caracteres permitidos 10"]);
            }

        }
        private void ValidateIdcountry(ConcurrentDictionary<string, List<string>> erros, string idCodeCountri)
        {
            // Any(char.IsDigit) valida que el nombre no tenga numeros

            bool exist = _countryModel.Countries.ContainsKey(idCodeCountri);

            if (!exist)
            {
                // Agrega la entrada al diccionario
                erros.TryAdd("idCodigoPais",
                    // El Select, recorre la lista y muestra todos los posibles valores
                    _countryModel.Countries
                    .Select(kv => $"Codigo esperado : {kv.Key}; Significado : {kv.Value}")
                    .ToList());
            }

        }

    }
}
