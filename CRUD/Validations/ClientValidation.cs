using CRUD.Models;
using CRUD.Models.bdCrud;
using CRUD.Models.CrudBD;

namespace CRUD.Validations
{
    public class ClientValidation
    {
        // Variables
        private readonly IdentificationTypeModel _identificationTypeStruct = new();
        private readonly InternalCode _internalCodes = new();

        // Funciones
        public ValidationModel Create(ClientModel client)
        {
            ValidationModel validation = new();
            Dictionary<string, List<string>> erros = [];

            try
            {

                ValidateName(erros, client.Nombre);
                ValidateAge(erros, client.Edad);
                ValidateIdentificationType(erros, client.IdTipoIdentificacion);
                ValidateIdentification(erros, client.NumeroIdentificacion);

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
        public ValidationModel ReadOrDelete(string identificationNumber)
        {

            ValidationModel validation = new();
            Dictionary<string, List<string>> erros = [];

            try
            {
                ValidateIdentification(erros, identificationNumber);

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
            return validation;
        }
        public ValidationModel Update(ClientModel client)
        {
            ValidationModel validation = new();
            Dictionary<string, List<string>> erros = [];

            try
            {
                ValidateId(erros, client.Id);
                ValidateName(erros, client.Nombre);
                ValidateAge(erros, client.Edad);
                ValidateIdentificationType(erros, client.IdTipoIdentificacion);
                ValidateIdentification(erros, client.NumeroIdentificacion);

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
                erros.Add("nombre", ["No puede estar vacio"]);
            }
            else if (name.Any(char.IsDigit))
            {
                erros.Add("nombre", ["No se aceptan numeros."]);
            }
            else if (name.Length > 100)
            {
                erros.Add("nombre", ["Numero Maximo de caracteres aceptados 100."]);
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
            // Valida si el tipo de identificación es valido
            bool isOk = _identificationTypeStruct.IdentificationTypes.ContainsKey(IdTipoIdentificacion);

            if (!isOk)
            {
                // Agrega la entrada al diccionario de errores
                erros.Add("idTipoIdentificacion",
                    // El Select, recorre la lista y muestra todos los posibles valores
                    _identificationTypeStruct.IdentificationTypes
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
    }
}
