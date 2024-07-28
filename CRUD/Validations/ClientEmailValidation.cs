using CRUD.Models;
using CRUD.Models.CrudBD;
using System.Text.RegularExpressions;

namespace CRUD.Validations
{
    public class ClientEmailValidation
    {
        // Variables
        private readonly CountryModel _countryModel = new();
        private readonly InternalCode _internalCodes = new();

        // Funciones
        public ValidationModel Create(ClientEmailModel clientEmail)
        {
            ValidationModel validation = new();
            Dictionary<string, List<string>> erros = [];

            try
            {

                ValidateId(erros, clientEmail.IdCliente);
                ValidateEmail(erros, clientEmail.CorreoElectronico);

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
        public ValidationModel ReadOrDelete(int idClient, string email = "")
        {

            ValidationModel validation = new();
            Dictionary<string, List<string>> erros = [];

            try
            {
                // Si el parametro opcional no fue diligenciado
                if (string.IsNullOrEmpty(email))
                {
                    ValidateId(erros, idClient);
                }
                // Si se diligencio
                else
                {
                    ValidateId(erros, idClient);
                    ValidateEmail(erros, email);
                }

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

        public ValidationModel Update(ClientEmailModel clientEmail)
        {
            ValidationModel validation = new();
            Dictionary<string, List<string>> erros = [];

            try
            {
                ValidateId(erros, clientEmail.Id);
                ValidateId(erros, clientEmail.IdCliente);
                ValidateEmail(erros, clientEmail.CorreoElectronico);

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
        private Dictionary<string, List<string>> ValidateId(Dictionary<string, List<string>> erros, int idClient)
        {
            if (idClient == 0) erros.Add("id", ["El id es requerido, su valor no puede ser 0"]);

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
            else if (email.Length > 100)
            {
                erros.Add("correoElectronico", ["Numero Maximo de caracteres aceptados 100."]);
            }

            return erros;

        }
    }
}
