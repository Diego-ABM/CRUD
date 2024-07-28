using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models.CrudBD
{
    [Table("cliente_correo_electronico")]
    public class ClientEmailModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Asegura que la columna id sea autoincremental
        [Column("id")]
        public int Id { get; set; }
        [Column("id_cliente")]
        public int IdCliente { get; set; }
        [Column("direccion_correo")]
        public string CorreoElectronico { get; set; } = string.Empty;
    }
}
