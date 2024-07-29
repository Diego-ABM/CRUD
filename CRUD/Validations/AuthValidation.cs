using CRUD.Models;
using CRUD.Validations.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace CRUD.Validations
{
    public class AuthValidation : IAuthValidation
    {
        // Variables
        private readonly InternalCode _internalCodes = new();

        // Funciones
        public async Task<ValidationModel> LoginAsync(LoginModel login)
        {
            // Inicio mi modelo de validación
            ValidationModel validation = new();
            // Utilizo un diccionary concurrente, para no manejar el bloqueo de hilos manualmente
            ConcurrentDictionary<string, List<string>> erros = [];

            try
            {
                // Crea lista de tareas
                List<Task> tasks = [
                    // Ejecutamos en otros hilos para agilizar las validaciónes
                    Task.Run(() => ValidateEmail(erros, login.CorreoElectronico)),
                    Task.Run(() => ValidatePassword(erros, login.Contrasenia))
                    ];


                // Espera a que las tareas terminen
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
                    validation.Message = "Request LoginAsync contiene errores";
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
            else if (email.Length > 20)
            {
                erros.TryAdd("correoElectronico", ["Numero Maximo de caracteres aceptados 100."]);
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
    }
}
