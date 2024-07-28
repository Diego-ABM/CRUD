using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models.CrudBD
{
    [Table("cliente_contacto")]
    public class ClientContactModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("id_cliente")]
        public int IdCliente { get; set; }

        [Column("id_codigo_pais", TypeName = "char(2)")]
        public string IdCodigoPais { get; set; } = string.Empty;

        [Column("tipo_telefono")]
        public string TipoTelefono { get; set; } = string.Empty;

        [Column("numero_telefono")]
        public string NumeroTelefono { get; set; } = string.Empty;
    }
}
