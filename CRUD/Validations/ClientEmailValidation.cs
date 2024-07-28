using CRUD.Models;
using CRUD.Models.CrudBD;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace CRUD.Validations
{
    public class ClientEmailValidation
    {
        // Variables
        private readonly CountryModel _countryModel = new();
        private readonly InternalCode _internalCodes = new();

        // Funciones
        public async Task<ValidationModel> CreateAsync(ClientEmailModel clientEmail)
        {
            ValidationModel validation = new();
            ConcurrentDictionary<string, List<string>> erros = [];

            try
            {
                // Crear una lista de tareas
                List<Task> tasks =
                    [
                        Task.Run(() => ValidateIdClient(erros, clientEmail.IdCliente)),
                        Task.Run(() => ValidateEmail(erros, clientEmail.CorreoElectronico))
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
        public async Task<ValidationModel> ReadOrDeleteAsync(int idClient, string email = "")
        {

            ValidationModel validation = new();
            ConcurrentDictionary<string, List<string>> erros = [];

            try
            {
                // Si el parametro opcional no fue diligenciado
                if (string.IsNullOrEmpty(email))
                {
                    await Task.Run(() => ValidateId(erros, idClient));
                }
                // Si se diligencio
                else
                {
                    List<Task> tasks =
                        [
                            Task.Run(() => ValidateId(erros, idClient)),
                            Task.Run(() => ValidateEmail(erros, email))
                        ];

                    await Task.WhenAll(tasks);
                }

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
            return validation;
        }
        public async Task<ValidationModel> UpdateAsync(ClientEmailModel clientEmail)
        {
            ValidationModel validation = new();
            ConcurrentDictionary<string, List<string>> erros = [];

            try
            {
                // Crear una lista de tareas
                List<Task> tasks =
                    [
                        Task.Run(() => ValidateId(erros, clientEmail.Id)),
                        Task.Run(() => ValidateIdClient(erros, clientEmail.IdCliente)),
                        Task.Run(() => ValidateEmail(erros, clientEmail.CorreoElectronico))
                    ];

                await Task.WhenAll(tasks);

                ValidateId(erros, clientEmail.Id);
                ValidateId(erros, clientEmail.IdCliente);
                ValidateEmail(erros, clientEmail.CorreoElectronico);

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

        // Validaciones
        private static void ValidateId(ConcurrentDictionary<string, List<string>> erros, int id)
        {
            if (id == 0) erros.TryAdd("id", ["El id es requerido, su valor no puede ser 0"]);

        }
        private static void ValidateIdClient(ConcurrentDictionary<string, List<string>> erros, int idClient)
        {
            if (idClient == 0) erros.TryAdd("idCliente", ["El id es requerido, su valor no puede ser 0"]);

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
