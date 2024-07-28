using CRUD.Models;
using CRUD.Models.CrudBD;
using System.Text.RegularExpressions;

namespace CRUD.Validations
{
    public class ClientContactValidation
    {
        // Variables
        private readonly CountryModel _countryModel = new();
        private readonly InternalCode _internalCodes = new();

        // Funciones
        public ValidationModel Create(ClientContactModel contact)
        {
            ValidationModel validation = new();
            Dictionary<string, List<string>> erros = [];

            try
            {

                ValidateId(erros, contact.IdCliente);
                ValidateIdcountry(erros, contact.IdCodigoPais);
                ValidatePhoneType(erros, contact.TipoTelefono);
                ValidatePhoneNumber(erros, contact.NumeroTelefono);

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
        public ValidationModel ReadOrDelete(int idClient)
        {

            ValidationModel validation = new();
            Dictionary<string, List<string>> erros = [];

            try
            {
                ValidateId(erros, idClient);

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
        public ValidationModel Update(ClientContactModel contact)
        {
            ValidationModel validation = new();
            Dictionary<string, List<string>> erros = [];

            try
            {
                ValidateId(erros, contact.Id);
                ValidateId(erros, contact.IdCliente);
                ValidateIdcountry(erros, contact.IdCodigoPais);
                ValidatePhoneType(erros, contact.TipoTelefono);
                ValidatePhoneNumber(erros, contact.NumeroTelefono);

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
        private Dictionary<string, List<string>> ValidateIdcountry(Dictionary<string, List<string>> erros, string idCodeCountri)
        {
            // Any(char.IsDigit) valida que el nombre no tenga numeros

            bool exist = _countryModel.Countries.ContainsKey(idCodeCountri);

            if (!exist)
            {
                // Agrega la entrada al diccionario
                erros.Add("idCodigoPais",
                    // El Select, recorre la lista y muestra todos los posibles valores
                    _countryModel.Countries
                    .Select(kv => $"Codigo valido : {kv.Key}; Significado : {kv.Value}")
                    .ToList());
            }

            return erros;
        }
        private Dictionary<string, List<string>> ValidatePhoneType(Dictionary<string, List<string>> erros, string phone)
        {
            // Valores aceptados en BD fijo o movil

            if (phone != "fijo" || phone != "movil")
            {
                erros.Add("tipoTelefono", ["Solo se aceptan valores fijo o movil"]);
            }

            return erros;
        }
        private Dictionary<string, List<string>> ValidatePhoneNumber(Dictionary<string, List<string>> erros, string phone)
        {
            // Expresion regular para validar que no tenga espacios en blanco
            string pattern = @"^\S+$";

            if (string.IsNullOrEmpty(phone))
            {
                erros.Add("numeroTelefono", ["No puede estar vacio"]);
            }
            else if (!Regex.IsMatch(phone, pattern))
            {
                erros.Add("numeroTelefono", ["No puede debe contener espacios en blanco."]);
            }
            else if (phone.Length > 15)
            {
                erros.Add("numeroTelefono", ["Numero maximo de caracteres permitidos 15."]);
            }
            else if (phone.Any(char.IsLetter))
            {
                erros.Add("numeroTelefono", ["Contiene texto"]);
            }

            return erros;
        }

    }
}
