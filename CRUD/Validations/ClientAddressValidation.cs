using CRUD.Models;
using CRUD.Models.CrudBD;

namespace CRUD.Validations
{
    public class ClientAddressValidation
    {
        // Variables
        private readonly CountryModel _countryModel = new();
        private readonly InternalCode _internalCodes = new();

        // Funciones
        public ValidationModel Create(ClientAddressModel clientAddress)
        {
            ValidationModel validation = new();
            Dictionary<string, List<string>> erros = [];

            try
            {

                ValidateId(erros, clientAddress.IdCliente);
                ValidateAddress(erros, clientAddress.Direccion);
                ValidateCity(erros, clientAddress.Ciudad);
                ValidateCodePostal(erros, clientAddress.CodigoPostal);
                ValidateIdcountry(erros, clientAddress.IdCodigoPais);


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
        public ValidationModel Update(ClientAddressModel clientAddress)
        {
            ValidationModel validation = new();
            Dictionary<string, List<string>> erros = [];

            try
            {
                ValidateId(erros, clientAddress.Id);
                ValidateId(erros, clientAddress.IdCliente);
                ValidateAddress(erros, clientAddress.Direccion);
                ValidateCity(erros, clientAddress.Ciudad);
                ValidateCodePostal(erros, clientAddress.CodigoPostal);
                ValidateIdcountry(erros, clientAddress.IdCodigoPais);

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
        private Dictionary<string, List<string>> ValidateAddress(Dictionary<string, List<string>> erros, string address)
        {

            if (string.IsNullOrEmpty(address))
            {
                // Agrega la entrada al diccionario
                erros.Add("direccion", ["No puede estar vacio"]);
            }
            if (address.Length > 255)
            {
                erros.Add("direccion", ["Suepera la cantidad maxima de caracteres permitidos 255"]);
            }

            return erros;
        }
        private Dictionary<string, List<string>> ValidateCity(Dictionary<string, List<string>> erros, string city)
        {

            if (string.IsNullOrEmpty(city))
            {
                // Agrega la entrada al diccionario
                erros.Add("ciudad", ["No puede estar vacio"]);
            }
            if (city.Length > 100)
            {
                erros.Add("ciudad", ["Supera la cantidad maxima de caracteres permitidos 100"]);
            }

            return erros;
        }
        private Dictionary<string, List<string>> ValidateCodePostal(Dictionary<string, List<string>> erros, string codePostal)
        {

            if (string.IsNullOrEmpty(codePostal))
            {
                // Agrega la entrada al diccionario
                erros.Add("ciudad", ["No puede estar vacio"]);
            }
            if (codePostal.Length > 10)
            {
                erros.Add("ciudad", ["Supera la cantidad maxima de caracteres permitidos 10"]);
            }

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
                    .Select(kv => $"Codigo esperado : {kv.Key}; Significado : {kv.Value}")
                    .ToList());
            }

            return erros;
        }

    }
}
