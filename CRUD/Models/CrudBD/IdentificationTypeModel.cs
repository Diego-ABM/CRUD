namespace CRUD.Models.CrudBD
{
    public class IdentificationTypeModel
    {





        public Dictionary<int, string> IdentificationTypes { get; set; }

        // Se delcara internamente para evitar consultas en BD
        public IdentificationTypeModel()
        {
            IdentificationTypes = new()
            {
                { 1 , "Cédula de Ciudadanía" },
                { 2 , "Cédula de Extranjería" },
                { 3 , "Número de Identificación Tributaria (NIT)" },
                { 4 , "Pasaporte" }
            };
        }

    }
}
