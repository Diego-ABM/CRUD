using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models.CrudBD
{
    [Table("cliente_direccion")]
    public class ClientAddressModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Asegura que la columna id sea autoincremental
        [Column("id")]
        public int Id { get; set; }
        [Column("id_cliente")]
        public int IdCliente { get; set; }
        [Column("direccion")]
        public string Direccion { get; set; } = string.Empty;
        [Column("ciudad")]
        public string Ciudad { get; set; } = string.Empty;
        [Column("codigo_postal")]
        public string CodigoPostal { get; set; } = string.Empty;
        [Column("id_codigo_pais")]
        public string IdCodigoPais { get; set; } = string.Empty;
    }
}
