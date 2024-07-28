using CRUD.Models.Interfaces;

namespace CRUD.Models
{
    public class Human : IHuman
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public byte Edad { get; set; }
        public int IdTipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; } = string.Empty;
    }
}
