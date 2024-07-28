namespace CRUD.Models.CrudBD.Structs
{
    public struct IdentificationTypeStruct
    {
        public Dictionary<int, string> IdentificationType { get; set; }

        public IdentificationTypeStruct()
        {
            IdentificationType = new()
            {
                { 1 , "Cédula de Ciudadanía" },
                { 2 , "Cédula de Extranjería" },
                { 3 , "Número de Identificación Tributaria (NIT)" },
                { 4 , "Pasaporte" }
            };
        }

    }
}
