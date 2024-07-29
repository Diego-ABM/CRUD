using CRUD.Models;
using CRUD.Models.CrudBD;
using CRUD.Validations.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace CRUD.Validations
{
    public class ClientContactValidation : IClientContactValidation
    {
        // Variables
        private readonly CountryModel _countryModel = new();
        private readonly InternalCode _internalCodes = new();

        // Funciones
        public async Task<ValidationModel> CreateAsync(ClientContactModel contact)
        {
            ValidationModel validation = new();
            ConcurrentDictionary<string, List<string>> erros = [];

            try
            {
                // Crear una lista de tareas
                List<Task> tasks =
                    [
                        Task.Run(() => ValidateIdClient(erros, contact.IdCliente)),
                        Task.Run(() => ValidateIdcountry(erros, contact.IdCodigoPais)),
                        Task.Run(() => ValidatePhoneType(erros, contact.TipoTelefono)),
                        Task.Run(() =>  ValidatePhoneNumber(erros, contact.NumeroTelefono))
                    ];

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
        public async Task<ValidationModel> ReadOrDeleteAsync(int idClient)
        {

            ValidationModel validation = new();
            ConcurrentDictionary<string, List<string>> erros = [];

            try
            {
                await Task.Run(() => ValidateId(erros, idClient));

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
        public async Task<ValidationModel> UpdateAsync(ClientContactModel contact)
        {
            ValidationModel validation = new();
            ConcurrentDictionary<string, List<string>> erros = [];

            try
            {
                // Crear una lista de tareas
                List<Task> tasks =
                    [
                        Task.Run(() => ValidateId(erros, contact.Id)),
                        Task.Run(() => ValidateIdClient(erros, contact.IdCliente)),
                        Task.Run(() => ValidateIdcountry(erros, contact.IdCodigoPais)),
                        Task.Run(() => ValidatePhoneType(erros, contact.TipoTelefono)),
                        Task.Run(() =>  ValidatePhoneNumber(erros, contact.NumeroTelefono))
                    ];

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

        // Metodos - Validaciones
        private static void ValidateId(ConcurrentDictionary<string, List<string>> erros, int id)
        {
            if (id == 0) erros.TryAdd("id", ["El id es requerido, su valor no puede ser 0"]);

        }
        private static void ValidateIdClient(ConcurrentDictionary<string, List<string>> erros, int idClient)
        {
            if (idClient == 0) erros.TryAdd("idCliente", ["El id es requerido, su valor no puede ser 0"]);

        }
        private void ValidateIdcountry(ConcurrentDictionary<string, List<string>> erros, string idCodeCountri)
        {
            // Valida si el codigo ingresado se encuentra en la lista
            bool exist = _countryModel.Countries.ContainsKey(idCodeCountri);

            if (!exist)
            {
                // Agrega la entrada al diccionario
                erros.TryAdd("idCodigoPais",
                    // El Select, recorre la lista y muestra todos los posibles valores
                    _countryModel.Countries
                    .Select(kv => $"Codigo valido : {kv.Key}; Significado : {kv.Value}")
                    .ToList());
            }
        }
        private static void ValidatePhoneType(ConcurrentDictionary<string, List<string>> erros, string phone)
        {
            // Valores aceptados en BD fijo o movil
            List<string> PhoneType = ["fijo", "movil"];

            if (!PhoneType.Contains(phone))
            {
                erros.TryAdd("tipoTelefono", ["Solo se aceptan valores fijo o movil"]);
            }



        }
        private static void ValidatePhoneNumber(ConcurrentDictionary<string, List<string>> erros, string phone)
        {
            // Expresion regular para validar que no tenga espacios en blanco
            string pattern = @"^\S+$";

            if (string.IsNullOrEmpty(phone))
            {
                erros.TryAdd("numeroTelefono", ["No puede estar vacio"]);
            }
            else if (!Regex.IsMatch(phone, pattern))
            {
                erros.TryAdd("numeroTelefono", ["No puede debe contener espacios en blanco."]);
            }
            else if (phone.Length > 15)
            {
                erros.TryAdd("numeroTelefono", ["Numero maximo de caracteres permitidos 15."]);
            }
            else if (phone.Any(char.IsLetter))
            {
                erros.TryAdd("numeroTelefono", ["Contiene texto"]);
            }

        }

    }
}
