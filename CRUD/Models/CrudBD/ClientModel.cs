using CRUD.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models.bdCrud
{
    public class ClientModel : IHuman
    {
        // Usamos data anotations para mapearlas a las columnas de BD

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Asegura que la columna id sea autoincremental
        [Column("id")]
        public int Id { get; set; }
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;
        [Column("edad")]
        public byte Edad { get; set; }
        [Column("id_tipo_identificacion")]
        public int IdTipoIdentificacion { get; set; }
        [Column("numero_identificacion")]
        public string NumeroIdentificacion { get; set; } = string.Empty;
    }
}
